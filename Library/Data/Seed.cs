using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Library.Models
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            // Create roles if they don't exist
            await CreateRolesAsync(roleManager);

            // Create users if they don't exist
            await CreateUsersAsync(userManager);
        }

        private static async Task CreateRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            }

            if (!await roleManager.RoleExistsAsync(UserRoles.User))
            {
                await roleManager.CreateAsync(new IdentityRole(UserRoles.User));
            }
        }

        private static async Task CreateUsersAsync(UserManager<AppUser> userManager)
        {
            // Create an admin user if it doesn't exist
            var adminUser = await userManager.FindByEmailAsync("phillipdeveloper@gmail.com");
            if (adminUser == null)
            {
                var newAdminUser = new AppUser()
                {
                    UserName = "phillipDev",
                    Email = "phillipdeveloper@gmail.com",
                    EmailConfirmed = true,
                };

                await userManager.CreateAsync(newAdminUser, "Coding@1234?");
                await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
            }

            // Create a regular user if it doesn't exist
            var appUser = await userManager.FindByEmailAsync("user@ebooks.com");
            if (appUser == null)
            {
                var newAppUser = new AppUser()
                {
                    UserName = "app-user",
                    Email = "user@ebooks.com",
                    EmailConfirmed = true,
                };

                await userManager.CreateAsync(newAppUser, "Coding@1234?");
                await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
            }
        }
    }
}
