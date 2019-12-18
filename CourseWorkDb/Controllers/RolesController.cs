using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseWorkDb.Models.Authentication;
using CourseWorkDb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CourseWorkDb.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        //смена ролей через админа
        public async Task<IActionResult> Edit(string userId)
        {
            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                IList<string> userRoles = await _userManager.GetRolesAsync(user);
                ChangeRoleViewModel model = new ChangeRoleViewModel
                {
                    UserId = user.Id,
                    UserEmail = user.Email,
                    AdminRole = Roles.Admin,
                    UserAdminRole = userRoles.FirstOrDefault(item => item.Equals(Roles.Admin)) == null ? "" : Roles.Admin
                };
                return View(model);
            }

            return NotFound();
        }
        [HttpPost]

        //редактирование ролей
        public async Task<IActionResult> Edit(string userId, string adminRole)
        {
            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                if (adminRole == null)
                {
                    await _userManager.RemoveFromRoleAsync(user, Roles.Admin);
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, Roles.Admin);
                }
                return RedirectToAction("Index", "Users");
            }

            return NotFound();
        }
    }
}