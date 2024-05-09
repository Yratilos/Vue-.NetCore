using System;
using System.Linq;
using System.Text;
using WebApi.Systems.Extensions;

namespace WebApi.Models
{
    ///<summary>
    ///
    ///</summary>
    public partial class Job
    {
           public Job(){

            this.ID =("'00000000-0000-0000-0000-000000000000'::uuid").ToGuid();
            this.CreateTime =("now()").ToDateTime();
            this.UpdateTime =("now()").ToDateTime();
            this.LogType =Convert.ToString("''::varchar");
            this.Content =Convert.ToString("''::varchar");
            this.Model =Convert.ToString("''::varchar");

           }
           /// <summary>
           /// Desc:主键
           /// Default:'00000000-0000-0000-0000-000000000000'::uuid
           /// Nullable:False
           /// </summary>           
           public Guid ID {get;set;}

           /// <summary>
           /// Desc:创建时间
           /// Default:now()
           /// Nullable:True
           /// </summary>           
           public DateTime? CreateTime {get;set;}

           /// <summary>
           /// Desc:修改时间
           /// Default:now()
           /// Nullable:True
           /// </summary>           
           public DateTime? UpdateTime {get;set;}

           /// <summary>
           /// Desc:类型
           /// Default:''::varchar
           /// Nullable:True
           /// </summary>           
           public string LogType {get;set;}

           /// <summary>
           /// Desc:内容
           /// Default:''::varchar
           /// Nullable:True
           /// </summary>           
           public string Content {get;set;}

           /// <summary>
           /// Desc:模块
           /// Default:''::varchar
           /// Nullable:True
           /// </summary>           
           public string Model {get;set;}

    }
}
