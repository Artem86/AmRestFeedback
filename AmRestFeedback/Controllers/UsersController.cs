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
            _logger = loggerFactory.CreateLogger<ManageController>();
        }

        public async Task<IActionResult> Index()
        {
            return View(await GetUsersVmAsync());
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
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
                    User = u,
                    Roles = r
                });
            }

            return usersVM;
        }

        public async Task<IActionResult> Edit(object userId)
        {
            var user = await _userManager.FindByIdAsync((string)userId);
            var roles = await _userManager.GetRolesAsync(user);
            var userVM = new UserViewModel
            {
                User = user,
                Roles = roles
            };
            return View("Edit", userVM);
        }
    }
}