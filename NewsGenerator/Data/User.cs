using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EntityGeneratorNews.Data
{   public class User : IBaseEntity
    {

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }  
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public Guid RoleId { get; set; }
        public virtual Role Roles { get; set; }        

    }
}
