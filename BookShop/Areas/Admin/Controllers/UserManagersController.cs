using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BookShop.Areas.Identity.Data;
using BookShop.Utilities;
using BookShop.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using ReflectionIT.Mvc.Paging;

namespace BookShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserManagersController : Controller
    {
        private readonly IApplicationUserManager _userManager;
        private readonly IApplicationRoleManager _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly ConvertDate _converDate;
        private readonly IHostingEnvironment _env;
        public UserManagersController(
            IEmailSender emailSender,
            IApplicationUserManager userManager,
            IApplicationRoleManager roleManager,
            ConvertDate converDate,
            IHostingEnvironment env)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _converDate = converDate;
            _emailSender = emailSender;
            _env = env;

        }
        public async Task<IActionResult> Index(string Msg, string sortExpression = "UserID", int pageSize = 5, int pageIndex = 1)
        {

            var users = await _userManager.GetAllUsersWithRolesAsync();

            var PagingModel = PagingList.Create(users, pageSize, pageIndex, sortExpression, "UserID");

            return View(PagingModel);
        }

        public async Task<IActionResult> Details(string id)
        {
            var user = await _userManager.FindUserWithRoleByIdAsync(id);
            return View(user);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
                return NotFound();
            var User = await _userManager.FindUserWithRoleByIdAsync(id);
            if (User == null)
                return NotFound();
            else
            {
                ViewBag.AllRoles = _roleManager.GetAllRoles();
                User.PersianBirthDate = (User.BirthDate == null) ? "" : _converDate.ToShamsi(User.BirthDate.Value, "yyyy/MM/dd");
                return View(User);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UsersViewModel ViewModel)
        {
            if (ModelState.IsValid)
            {
                var User = await _userManager.FindByIdAsync(ViewModel.UserID);
                if (User == null)
                    return NotFound();
                else
                {
                    IdentityResult Result;
                    var RecentRoles = await _userManager.GetRolesAsync(User);
                    var DeleteRoles = RecentRoles.Except(ViewModel.Roles);
                    var AddRoles = ViewModel.Roles.Except(RecentRoles);

                    Result = await _userManager.RemoveFromRolesAsync(User, DeleteRoles);
                    if (Result.Succeeded)
                    {
                        Result = await _userManager.AddToRolesAsync(User, AddRoles);
                        if (Result.Succeeded)
                        {
                            User.FirstName = ViewModel.FirstName;
                            User.LastName = ViewModel.LastName;
                            User.Email = ViewModel.Email;
                            User.PhoneNumber = ViewModel.Mobile;
                            if (ViewModel.Image!=null)
                            {
                                var fileName = User.Id + Path.GetExtension(ViewModel.Image.FileName);
                                var FilePath = Path.Combine(_env.WebRootPath, "imgs", fileName);
                                using (var s = new FileStream(FilePath,FileMode.Create))
                                {
                                   await ViewModel.Image.CopyToAsync(s);
                                }
                                User.Image = fileName;
                            }
                            User.UserName = ViewModel.UserName;
                            if (ViewModel.PersianBirthDate != null)
                                User.BirthDate = _converDate.ToMiladi(ViewModel.PersianBirthDate);

                            Result = await _userManager.UpdateAsync(User);
                            if (Result.Succeeded)
                            {
                                ViewBag.AlertSuccess = "ذخیره تغییرات با موفقیت انجام شد.";
                            }
                        }
                    }

                    if (Result != null)
                    {
                        foreach (var item in Result.Errors)
                        {
                            ModelState.AddModelError("", item.Description);
                        }
                    }
                }
            }

            ViewBag.AllRoles = _roleManager.GetAllRoles();
            return View(ViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
                return NotFound();
            var User = await _userManager.FindByIdAsync(id);
            if (User == null)
                return NotFound();
            else
                return View(User);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> Deleted(string id)
        {
            if (id == null)
                return NotFound();
            var User = await _userManager.FindByIdAsync(id);
            if (User == null)
                return NotFound();
            else
            {
                var Result = await _userManager.DeleteAsync(User);
                if (Result.Succeeded)
                    return RedirectToAction("Index");
                else
                    ViewBag.AlertError = "در حذف اطلاعات خطایی رخ داده است.";

                return View(User);
            }
        }

        public async Task<IActionResult> SendEmail(string[] emails, string subject, string message)
        {
            if (emails != null)
            {
                for (int i = 0; i < emails.Length; i++)
                {
                    await _emailSender.SendEmailAsync(emails[i], subject, message);
                }
            }
            return RedirectToAction(nameof(Index), new { Msg = "SendEmail" });
        }

    }
}