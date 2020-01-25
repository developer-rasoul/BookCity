using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Areas.Identity.Data
{
    public class ApplicationRole :IdentityRole
    {
        public ApplicationRole()
        {

        }
        public ApplicationRole(string name) : base(name)
        {

        }
        public ApplicationRole(string name, string desc):base(name)
        {
            Description = desc;
        }


        public string Description { get; set; }

        public virtual List<ApplicationUserRole> UserRoles { get; set; }

    }
}
