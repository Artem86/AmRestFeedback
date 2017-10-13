using AmRestFeedback.Enums;
using AmRestFeedback.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AmRestFeedback.Data
{
    public class DbInitializer
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            var context = services.GetService<ApplicationDbContext>();
            var userManager = services.GetService<UserManager<ApplicationUser>>();

            var roles = new List<IdentityRole> {
                new IdentityRole {
                    Name = nameof(RolesEnum.Administrator),
                    NormalizedName = nameof(RolesEnum.Administrator).ToUpper()
                },
                new IdentityRole {
                    Name = nameof(RolesEnum.Manager),
                    NormalizedName = nameof(RolesEnum.Manager).ToUpper()
                },
            };

            foreach (var role in roles)
            {
                var roleStore = new RoleStore<IdentityRole>(context);

                if (!context.Roles.Any(r => r.Name == role.Name))
                {
                    await roleStore.CreateAsync(role);
                }
            }


            var user = new ApplicationUser
            {
                Email = "artem.kiyashko@amrest.eu",
                NormalizedEmail = "ARTEM.KIYASHKO@AMREST.EU",
                UserName = "artem.kiyashko@amrest.eu",
                NormalizedUserName = "ARTEM.KIYASHKO@AMREST.EU",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };


            if (!context.Users.Any(u => u.UserName == user.UserName))
            {
                var result = userManager.CreateAsync(user, "1Qazxsw@");
                if (result.Result.Succeeded)
                    await AssignRoles(services, user.NormalizedEmail, roles);
            }
            await context.SaveChangesAsync();
        }

        public static async Task AssignRoles(IServiceProvider services, string email, IEnumerable<IdentityRole> roles)
        {
            UserManager<ApplicationUser> _userManager = services.GetService<UserManager<ApplicationUser>>();
            ApplicationUser user = await _userManager.FindByEmailAsync(email);

            await _userManager.AddToRolesAsync(user, roles.Select(_ => _.Name));
        }
    }
}
