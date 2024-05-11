using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var Configuration = new ConfigurationBuilder().SetBasePath(Environment.CurrentDirectory)
                                          .AddJsonFile("appsettings.json")
                                          .Build();
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseConfiguration(Configuration);
                    webBuilder.UseStartup<Startup>();
                });
        }
    }
}
