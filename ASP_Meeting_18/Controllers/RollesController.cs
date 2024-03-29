﻿using ASP_Meeting_18.Data;
using ASP_Meeting_18.Models.ViewModels.RollesViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ASP_Meeting_18.Controllers
{
    public class RollesController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public RollesController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public async Task< IActionResult> Index()
        {
            return View(await roleManager.Roles.ToListAsync());
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                IdentityRole newRole = new IdentityRole(name);
                IdentityResult result = await roleManager.CreateAsync(newRole);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "The role`s name can not be empty!");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            IdentityRole role = await roleManager.FindByIdAsync(id);
            if(role != null)
            {
                await roleManager.DeleteAsync(role);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> UserList()
        {
            var users = await userManager.Users.ToListAsync();
            return View(users);
        }

        public async Task<IActionResult> ChangeUserRoles(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            User user = await userManager.FindByIdAsync(id); 
            if (user == null)
            {
                return NotFound();
            }
            IList<string> userRoles = await userManager.GetRolesAsync(user);
            List<IdentityRole> allRoles = await roleManager.Roles.ToListAsync();
            ChangeRollesViewModel vm = new ChangeRollesViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                UserRoles = userRoles,
                AllRoles = allRoles
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUserRoles(string? id, List<string> roles)
        {
            if(id == null)
            {
                return NotFound();
            }
            User user = await userManager.FindByIdAsync(id);
            if(user == null)
            {
                return NotFound();
            }
            var userRoles = await userManager.GetRolesAsync(user);
            List<string> allRoles = await roleManager.Roles.Select(r => r.Name).ToListAsync();
            IEnumerable<string> addedRoles = roles.Except(userRoles);
            IEnumerable<string> deleteRoles = userRoles.Except(roles);
            await userManager.AddToRolesAsync(user, addedRoles);
            await userManager.RemoveFromRolesAsync(user, deleteRoles);
            return RedirectToAction("UserList");
        }

        //public Task<IActionResult> GetChildCategories(string parentCategoryId)
        //{
        //    return PartialView(); 
        //}
    }
}