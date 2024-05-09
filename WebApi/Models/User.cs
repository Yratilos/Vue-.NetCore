using System;
using System.Linq;
using System.Text;
using WebApi.Systems.Extensions;

namespace WebApi.Models
{
    ///<summary>
    ///
    ///</summary>
    public partial class User
    {
           public User(){

            this.ID =("'00000000-0000-0000-0000-000000000000'::uuid").ToGuid();
            this.Name =Convert.ToString("''::varchar");
            this.Account =Convert.ToString("''::varchar");
            this.CreateTime =("now()").ToDateTime();
            this.UpdateTime =("now()").ToDateTime();
            this.Password =Convert.ToString("''::varchar");

           }
           /// <summary>
           /// Desc:
           /// Default:'00000000-0000-0000-0000-000000000000'::uuid
           /// Nullable:False
           /// </summary>           
           public Guid ID {get;set;}

           /// <summary>
           /// Desc:
           /// Default:''::varchar
           /// Nullable:True
           /// </summary>           
           public string Name {get;set;}

           /// <summary>
           /// Desc:
           /// Default:''::varchar
           /// Nullable:True
           /// </summary>           
           public string Account {get;set;}

           /// <summary>
           /// Desc:
           /// Default:now()
           /// Nullable:True
           /// </summary>           
           public DateTime? CreateTime {get;set;}

           /// <summary>
           /// Desc:
           /// Default:now()
           /// Nullable:True
           /// </summary>           
           public DateTime? UpdateTime {get;set;}

           /// <summary>
           /// Desc:
           /// Default:''::varchar
           /// Nullable:True
           /// </summary>           
           public string Password {get;set;}

    }
}
