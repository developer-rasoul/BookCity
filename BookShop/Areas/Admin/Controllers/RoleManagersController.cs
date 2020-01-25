using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookShop.Areas.Identity.Data;
using BookShop.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;

namespace BookShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleManagersController : Controller
    {
        private readonly IApplicationRoleManager _roleManager;
        public RoleManagersController(IApplicationRoleManager roleManager)
        {
            _roleManager = roleManager;
        }
        public IActionResult Index(int pageSize = 3, int currentPage = 1, string sortExpression = "RoleName")
        {
            var roles = _roleManager.GetAllRolesAndUsersCount();
            var PagingModel = PagingList.Create(roles, pageSize, currentPage, "RoleName", "RoleName");
            PagingModel.RouteValue = new Microsoft.AspNetCore.Routing.RouteValueDictionary
            {
                {"pageSize",pageSize }
            };

            return View(PagingModel);
        }

        public IActionResult AddRole()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRole(RolesViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var Result = await _roleManager.CreateAsync(new ApplicationRole(viewModel.RoleName, viewModel.Description));
                if (Result.Succeeded)
                {
                    TempData["Success"] = "نقش با موفقیت اضافه شد.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Success"] = null;
                    if (await _roleManager.RoleExistsAsync(viewModel.RoleName))
                    {
                        ViewBag.Error = "نقش در برنامه موجود است. نقش جدید وارد کنید ...";
                    }
                    else
                    {
                        ViewBag.Error = "در ذخیره اطلاعات مشکلی به وجود آمد. دوباره سعی کنید ...";
                    }
                }
            }
            return View(viewModel);
        }


        public async Task<IActionResult> EditRole(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            RolesViewModel rolevm = new RolesViewModel { RoleID = role.Id, RoleName = role.Name, Description = role.Description };

            return View(rolevm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRole(RolesViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(viewModel.RoleID);
                if (role == null)
                {
                    return NotFound();
                }
                if (await _roleManager.RoleExistsAsync(viewModel.RoleName) && role.Name != viewModel.RoleName)
                {
                    ViewBag.Error = "نقش در برنامه موجود است. نقش جدید وارد کنید ...";
                }
                else
                {
                    TempData["Success"] = null;
                    role.Name = viewModel.RoleName;
                    role.Description = viewModel.Description;
                    var Result = await _roleManager.UpdateAsync(role);
                    if (Result.Succeeded)
                    {
                        TempData["Success"] = "نقش با موفقیت ویرایش شد.";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Error = "در ذخیره اطلاعات مشکلی به وجود آمد. دوباره سعی کنید ...";
                    }
                }
            }
            return View(viewModel);
        }

        public async Task<IActionResult> DeleteRole(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            RolesViewModel roleVM = new RolesViewModel { RoleID = role.Id, RoleName = role.Name };
            return View(roleVM);
        }
        [HttpPost]
        [ActionName("DeleteRole")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletedRole(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            await _roleManager.DeleteAsync((role));
            return RedirectToAction("Index");
        }


    }
}