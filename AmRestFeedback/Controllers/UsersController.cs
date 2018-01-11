using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AmRestFeedback.Models.UsersViewModels;
using Microsoft.AspNetCore.Identity;
using AmRestFeedback.Models;
using Microsoft.Extensions.Logging;
using AmRestFeedback.Enums;
using Microsoft.AspNetCore.Authorization;

namespace AmRestFeedback.Controllers
{
    [Authorize(Roles = nameof(RolesEnum.Administrator))]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;

        public UsersController(
          UserManager<ApplicationUser> userManager,
          ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _logger = loggerFactory.CreateLogger<UsersController>();
        }

        public async Task<IActionResult> Index()
        {
            return View(await GetUsersVmAsync());
        }

        private async Task<ApplicationUser> GetCurrentUserAsync()
        {
            return await _userManager.GetUserAsync(HttpContext.User);
        }

        private async Task<IEnumerable<UserViewModel>> GetUsersVmAsync()
        {
            var usersVM = new List<UserViewModel>();
            var currentUserRoles = await _userManager.GetRolesAsync(await GetCurrentUserAsync());
            if (!currentUserRoles.Contains(nameof(RolesEnum.Administrator)))
                return usersVM;

            var allUsers = _userManager.Users.ToList();
            foreach (var u in allUsers)
            {
                var r = await _userManager.GetRolesAsync(u);
                usersVM.Add(new UserViewModel
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    Roles = string.Join(", ", r)
                });
            }

            return usersVM;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserViewModel userVm)
        {
            var user = await _userManager.FindByIdAsync(userVm.Id);
            var roles = await _userManager.GetRolesAsync(user);
            return View("Index", await GetUsersVmAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var roles = await _userManager.GetRolesAsync(user);
            var rolesVms = new List<EditRoleViewModel>();
            foreach(var role in roles)
            {
                rolesVms.Add(new EditRoleViewModel
                {
                    UserId = user.Id,
                    Role = role
                });
            }
            var userVM = new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Roles = string.Join(", ", roles),
                RolesList = rolesVms
            };
            return View("Edit", userVM);
        }
    }
}