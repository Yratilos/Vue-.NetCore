using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using WebApi.IUtils;
using WebApi.Systems;
using WebApi.Systems.DataBases;
using WebApi.Systems.Extensions;
using WebApi.Systems.Filters;
using WebApi.Utils;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var urls = Configuration["Cors"].TrimEnd(';').Split(';')
                                 .Where(url => url.StartsWith("http:", StringComparison.OrdinalIgnoreCase))
                                 .ToArray();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", builder =>
                {
                    builder.WithOrigins(urls)
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials();
                });
            });
            services.AddControllers(options =>
            {
                options.Filters.Add<AuthorizationFilter>();
                options.Filters.Add<ActionFilter>();
                options.Filters.Add<ExceptionFilter>();
                options.Filters.Add<ResultFilter>();
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Web API", Version = "v1" });
                //c.SwaggerDoc("v2", new OpenApiInfo { Title = "Web API", Version = "v2" });

                // ָ��Swagger��ȡXML�ĵ�ע��
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                if (Configuration["Jwt:Enabled"].ToBoolean())
                {
                    var securityScheme = new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Description = "JWT Authorization header using the Bearer scheme",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer"
                    };

                    c.AddSecurityDefinition("Bearer", securityScheme);

                    var securityRequirement = new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                        }
                    };

                    c.AddSecurityRequirement(securityRequirement);
                }
            });

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            var dataBase = Configuration["DataBase"];
            services.AddSingleton(DataBaseFactory.CreateDataBase(dataBase, Configuration["ConnectionStrings:" + dataBase]));
            //services.AddDbContext<EFDbContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:SqlServer"]));
            //services.AddSingleton(SqlSugarCore.GetInstance(Configuration["ConnectionStrings:" + dataBase], dataBase));

            services.AddSingleton<IFileUtil, FileUtil>();
            services.AddSingleton<IHttpUtil, HttpUtil>();
            services.AddSingleton<IJwtUtil, JwtUtil>();
            services.AddSingleton<IWebSocketUtil, WebSocketUtil>();
            services.AddSingleton<ILogger, Logger>();

            services.AddHttpContextAccessor();
            services.AddDistributedMemoryCache(); // ����ʹ�������ֲ�ʽ�����ṩ������ Redis
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(Configuration["SessionTimeout"].ToDouble()); // ���ûỰ��ʱʱ��
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            services.AddScoped<CustomSession>();

            services.AddMemoryCache(options =>
            {
                options.ExpirationScanFrequency = TimeSpan.FromSeconds(Configuration["Cache:Timeout"].ToDouble()); // ���û��泬ʱʱ��
            });
            services.AddSingleton<CustomCache>();

            var assembly = Assembly.GetExecutingAssembly();

            // �ҵ�����ʵ����Service�ӿڵ�����
            var serviceTypes = assembly.GetTypes()
                .Where(type => (typeof(IService).IsAssignableFrom(type) || (!string.IsNullOrEmpty(type.Namespace) && type.Namespace.Contains("Services"))) && !type.IsInterface);

            // ע�������
            foreach (var serviceType in serviceTypes)
            {
                // ��ȡ��Ӧ��IService�ӿ�����
                var interfaceType = serviceType.GetInterfaces()
                    .FirstOrDefault(i => i != typeof(IService));

                if (interfaceType != null)
                {
                    services.AddScoped(interfaceType, serviceType);
                }
            }

            // �ҵ�����ʵ����Repository�ӿڵ����ͺͶ�Ӧ�����ݿ���
            var repositoryTypes = assembly.GetTypes()
                .Where(type => (typeof(IRepository).IsAssignableFrom(type) || (!string.IsNullOrEmpty(type.Namespace) && type.Namespace.Contains(dataBase))) && !type.IsInterface);

            // ע��ִ���
            foreach (var repositoryType in repositoryTypes)
            {
                // ��ȡ��Ӧ��IRepository�ӿ�����
                var interfaceType = repositoryType.GetInterfaces()
                    .FirstOrDefault(i => i != typeof(IRepository));

                if (interfaceType != null)
                {
                    services.AddScoped(interfaceType, repositoryType);
                }
            }

            if (Configuration["Jwt:Enabled"].ToBoolean())
            {
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "JwtBearer";
                    options.DefaultChallengeScheme = "JwtBearer";
                }).AddJwtBearer("JwtBearer", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
                        RequireExpirationTime = true
                    };
                });
            }
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
                    //c.SwaggerEndpoint("/swagger/v2/swagger.json", "API v2");
                    c.DocExpansion(DocExpansion.List); // �޸�������Ĭ��չ����ʽ
                    c.RoutePrefix = "swagger"; // �޸�Swagger UI��·��ǰ׺
                    c.DocumentTitle = Configuration["DocumentTitle"];//��ҳ����
                });
            }
            else
            {
                app.UseDefaultFiles(new DefaultFilesOptions
                {
                    DefaultFileNames = new List<string> { "index.html" }
                });
                app.UseStaticFiles();
            }

            var supportedCultures = new[]
            {
                new CultureInfo("en-US"),
                new CultureInfo("zh-CN"),
            };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("zh-CN"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            app.UseRouting();

            app.UseCors("AllowSpecificOrigin");

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseSession();

            //app.UseStaticFiles();//ʹ�þ�̬�ļ�

            //��ͻ��˷��͡�ping��
            app.UseWebSockets(new WebSocketOptions
            {
                KeepAliveInterval = TimeSpan.FromSeconds(Configuration["WebSocketAliveInterval"].ToDouble())
            });

            app.UseMiddleware<WebSocketUtilMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
