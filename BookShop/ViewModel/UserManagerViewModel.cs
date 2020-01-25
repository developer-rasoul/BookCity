using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.ViewModel
{
    public class UsersViewModel
    {
        public string UserID { get; set; }

        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        public string UserName { get; set; }

        [Display(Name = "ایمیل")]
        public string Email { get; set; }

        [Display(Name = "موبایل")]
        public string Mobile { get; set; }
        
        [Display(Name = "نام")]
        public string FirstName { get; set; }
        
        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; }
        
        [Display(Name = "تاریخ تولد")]
        public DateTime? BirthDate { get; set; }
        
        [Display(Name = "عکس پروفایل")]
        public string ImagePath { get; set; }
        public IFormFile Image { get; set; }

        [Display(Name = "آخرین بازدید")]
        public DateTime? LastVisit { get; set; }

        [Display(Name = "تاریخ ثبت نام")]
        public DateTime? RegisterDate { get; set; }

        [Display(Name = "وضعیت")]
        public bool IsActive { get; set; }

        [Display(Name = "نقش‌ها")]
        public IEnumerable<string> Roles { get; set; }

        [Display(Name = "تایید موبایل")]
        public bool ConfrmMobile { get; set; }

        [Display(Name = "تایید ایمیل")]
        public bool ConfrmEmail { get; set; }

        [Display(Name = "ثبت نام 2 مرحله‌ای")]
        public bool TwoFactorRegister { get; set; }

        [Display(Name = "قفل")]
        public bool IsLock { get; set; }

        [Display(Name = "تعداد ورود ناموفق")]
        public int AccessFailedCount { get; set; }

        [Display(Name = "تاریخ خروج از قفل")]
        public DateTimeOffset? EndLockOut { get; set; }
        public string PersianBirthDate { get; set; }
    }
}
