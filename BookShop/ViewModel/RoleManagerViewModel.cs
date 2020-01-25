using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.ViewModel
{
    public class RolesViewModel
    {
        public string RoleID { get; set; }

        [Display(Name = "نام نقش")]
        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        public string RoleName { get; set; }
        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        public int UserCount { get; set; }
    }
}
