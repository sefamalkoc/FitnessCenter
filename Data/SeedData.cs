using Microsoft.AspNetCore.Identity;
using FitnessCenter.Models;

namespace FitnessCenter.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roleNames = { "Admin", "Member" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Create Admin User
            var adminEmail = "ogrencinumarasi@sakarya.edu.tr";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "System Admin",
                    EmailConfirmed = true
                };

                var createPowerUser = await userManager.CreateAsync(adminUser, "sau"); // "sau" is password, but strictly it might not meet complex requirements. We'll see.
                // Identity default password policy requires lowercase, uppercase, digit, non-alphanumeric. 
                // "sau" is too simple. We might need to relax password policy in Program.cs OR use a stronger password.
                // The user SPECIFICALLY requested "sau". I will relax the policy in Program.cs.

                if (createPowerUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}
