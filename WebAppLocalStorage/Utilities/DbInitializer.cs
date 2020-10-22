using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace WebAppLocalStorage.Utilities
{
    public class DbInitializer
    {
        private static UserManager<IdentityUser> _userManager;
        private static RoleManager<IdentityRole> _roleManager;
        private static ILogger<DbInitializer> _logger;

        public static async Task Initialize(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<DbInitializer> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;

            var defaultUser = await CreateDefaultUser(_userManager, _logger, "ecr@live.dk", "P@ssw0rd");
            await CreateRole(_roleManager, _logger, "Admin");

            if (!await _userManager.IsInRoleAsync(defaultUser, "Admin"))
            {
                await _userManager.AddToRoleAsync(defaultUser, "admin");
            }
        }

        private static async Task CreateRole(RoleManager<IdentityRole> roleManager, ILogger<DbInitializer> logger, string name)
        {
            if (!await _roleManager.RoleExistsAsync(name))
            {
                IdentityResult result = await roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    logger.LogDebug($"********** Created the role `{name}` successfully");
                }
                else
                {
                    ApplicationException exception = new ApplicationException($"********** Default role `{name}` cannot be created");
                    throw exception;
                }
            }
        }

        private static async Task<IdentityUser> CreateDefaultUser(UserManager<IdentityUser> userManager, ILogger<DbInitializer> logger, string email, string password)
        {
            if (await _userManager.FindByNameAsync(email) == null)
            {
                IdentityResult identityResult = await userManager.CreateAsync(new IdentityUser { UserName = email, Email = email }, password);

                if (identityResult.Succeeded)
                {
                    logger.LogDebug($"********** Created default user `{email}` successfully");
                }
                else
                {
                    ApplicationException exception = new ApplicationException($"********** Default user `{email}` cannot be created");
                    throw exception;
                }
            }

            IdentityUser createdUser = await userManager.FindByEmailAsync(email);

            return createdUser;
        }
    }
}
