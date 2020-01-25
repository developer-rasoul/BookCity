using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using BookShop.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using BookShop.ViewModel;
using BookShop.Utilities;

namespace BookShop.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IApplicationRoleManager _roleManager;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            ILogger<RegisterModel> logger,
            IApplicationRoleManager roleManager)
        {
            _userManager = userManager;
            _logger = logger;
            _roleManager = roleManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IEnumerable<ApplicationRole> Roles { get; set; }
        [BindProperty]
        public string[] UserRoles { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
            [EmailAddress(ErrorMessage = "ایمیل وارد شده صحیح نیست.")]
            [Display(Name = "ایمیل")]
            public string Email { get; set; }

            [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
            [StringLength(100, ErrorMessage = "{0} باید حداقل {2} کاراکتر و حداکثر {1} کاراکتر داشته باشد.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "کلمه عبور")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "تکرار کلمه عبور")]
            [Compare("Password", ErrorMessage = "{0} با کلمه عبور مطابقت ندارد")]
            public string ConfirmPassword { get; set; }

            [Display(Name = "شماره موبایل")]
            public string Mobile { get; set; }
            [Display(Name = "نام")]
            public string FirstName { get; set; }
            [Display(Name = "نام خانوادگی")]
            public string LastName { get; set; }
            [Display(Name = "تاریخ تولد")]
            public string BirthDate { get; set; }
            [Display(Name = "نام کاربری")]
            public string UserName { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            Roles = _roleManager.GetAllRoles();
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/Admin/UserManagers?Msg=Success");
            if (ModelState.IsValid)
            {
                ConvertDate convertDate = new ConvertDate();
                DateTime? bDate = null;
                if (Input.BirthDate!=null)
                {
                    bDate = convertDate.ToMiladi(Input.BirthDate);
                }
                var user = new ApplicationUser
                {
                    UserName = Input.UserName,
                    Email = Input.Email,
                    FirstName=Input.FirstName,
                    LastName=Input.LastName,
                    BirthDate= bDate,
                    IsActive=true,
                    EmailConfirmed=true,
                    PhoneNumber= Input.Mobile,
                    RegisterDate=DateTime.Now,
                };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    if (UserRoles!=null)
                    {
                       await _userManager.AddToRolesAsync(user, UserRoles);
                    }
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            Roles = _roleManager.GetAllRoles();

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
