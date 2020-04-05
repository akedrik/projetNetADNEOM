using Microsoft.AspNetCore.Identity;
using NetCoreApp.Core.Classes.Constantes;
using System.Threading.Tasks;

namespace NetCoreApp.Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await CreateRoles(roleManager);
            await CreateDefaultUser(userManager);
            await CreateAdminUser(userManager);
        }

        private static async Task CreateAdminUser(UserManager<ApplicationUser> userManager)
        {
            string adminUserName = "admin@mail.com";
            var adminUser = await userManager.FindByNameAsync(adminUserName);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser { UserName = adminUserName, Email = adminUserName };
                await userManager.CreateAsync(adminUser, AuthorizationConstants.DEFAULT_PASSWORD);
                adminUser = await userManager.FindByNameAsync(adminUserName);
                await userManager.AddToRoleAsync(adminUser, AuthorizationConstants.Roles.ADMINISTRATORS);
            }
        }

        private static async Task CreateDefaultUser(UserManager<ApplicationUser> userManager)
        {
            string defaultUserName = "user@mail.com";
            var defaultUser = await userManager.FindByNameAsync(defaultUserName);
            if (defaultUser == null)
            {
                defaultUser = new ApplicationUser { UserName = defaultUserName, Email = defaultUserName };
                await userManager.CreateAsync(defaultUser, AuthorizationConstants.DEFAULT_PASSWORD);
                await userManager.AddToRoleAsync(defaultUser, AuthorizationConstants.Roles.CUSTOMERS);
            }
        }

        private static async Task CreateRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(AuthorizationConstants.Roles.ADMINISTRATORS))
                await roleManager.CreateAsync(new IdentityRole(AuthorizationConstants.Roles.ADMINISTRATORS));

            if (!await roleManager.RoleExistsAsync(AuthorizationConstants.Roles.CUSTOMERS))
                await roleManager.CreateAsync(new IdentityRole(AuthorizationConstants.Roles.CUSTOMERS));
        }
    }
}
