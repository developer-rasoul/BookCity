using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.ViewModel
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="وارد کردن {0} الزامی است.")]
        [Display(Name ="نام کاربری")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "وارد کردن {0} الزامی است.")]
        [Display(Name = "ایمیل"),EmailAddress(ErrorMessage ="یک ایمیل معتبر وارد کنید.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "وارد کردن {0} الزامی است.")]
        [Display(Name = "رمز عبور")]
        public string Password { get; set; }

        [Required(ErrorMessage = "وارد کردن {0} الزامی است.")]
        [Display(Name = "تایید رمز عبور"),Compare("Password", ErrorMessage ="رمز عبور و {0} یکسان نیست.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "وارد کردن {0} الزامی است.")]
        [Display(Name = "شماره تماس"),Phone(ErrorMessage ="یک شماره معتبر وارد کنید.")]
        public string PhoneNumber { get; set; }

    }
    public class SignInViewModel
    {
        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        [Display(Name = "نام کاربری")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        [DataType(DataType.Password)]
        [Display(Name = "کلمه عبور")]
        public string Password { get; set; }

        [Display(Name = "مرا به خاطر بسپار")]
        public bool RememberMe { get; set; }

        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        [StringLength(4, ErrorMessage = "کد امنیتی باید دارای 4 کاراکتر باشد.")]
        [Display(Name = "کد امنیتی")]
        public string CaptchaCode { get; set; }

    }
}
