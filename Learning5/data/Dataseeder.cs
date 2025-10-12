using Learning5.Models.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Learning5.data
{
    public class Dataseeder
    {
            public static async Task SeedAdminUser(IServiceProvider serviceProvider)
            {
                using var scope = serviceProvider.CreateScope();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Roles>>();
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

          
                await dbContext.Database.MigrateAsync();

                if (!await dbContext.Users.AnyAsync())
                {
                    string adminRole = "Admin";
                    Roles role = new Roles { Name = adminRole, NormalizedName = adminRole.ToUpper() };

                if (!await roleManager.RoleExistsAsync(adminRole))
                    {
                        await roleManager.CreateAsync(role);
                    }

                    var adminUser = new User
                    {
                        UserName = "E10001",
                        EmployeeName = "Admin1",
                        Email = "admin@example.com",
                        EmailConfirmed = true 
                    };

                    string adminPassword = "E10001Hrms@123";
                    var userResult = await userManager.CreateAsync(adminUser, adminPassword);

                    if (userResult.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, adminRole);
                    }
                    else
                    {
                        throw new Exception($"Failed to create admin user: {string.Join(", ", userResult.Errors.Select(e => e.Description))}");
                    }
                }
           
            }
    }
}
