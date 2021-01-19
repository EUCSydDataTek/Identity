# 4a.InitialAdminRoleUser
[Yogihosting.com](https://www.yogihosting.com/aspnet-core-identity-roles/)

## Oprettelse af ressourcerne i Program.cs
Oprettelsen af faste brugere (f.eks. en administrator) og en Admin-rolle skal ske som noget af det allerførste når 
applikationen starter op. Derfor sker det i Main() i Program-klassen. Bemærk også at der laves en Migration, således
at man er sikker på at databasen er oprettet inden man går videre:
```csharp
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting; 
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using WebAppLocalStorage.Data;
using WebAppLocalStorage.Utilities;

namespace WebAppLocalStorage
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    var dbInitializerLogger = services.GetRequiredService<ILogger<DbInitializer>>();
                    var configuration = services.GetRequiredService<IConfiguration>();
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    context.Database.Migrate();

                    await DbInitializer.Initialize(userManager, roleManager, dbInitializerLogger, configuration);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "********** An error occurred while migrating the database.");
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

```
&nbsp;

#### DbInitializer

Opret  klassen **DbInitializer**, som benytter `RoleManager` og `UserManager` til at oprette en fast rolle *Admin* og en fast bruger,
som er medlem af denne rolle.

Bemærk at credentials for admin brugeren ikke er hardkodet, men hentes vha. `Secret Manager`, der kan læse en `secret.json` fil fra den lokale maskine.
Læse mere om User Secret. Her er skabelonen for User Secret:

```json
{
  "AdminUser": 
  {
    "UserId": "userId",
    "AdminPassword": "password"
  },
}
```

&nbsp;


```csharp
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
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
        private static IConfiguration _config;

        public static async Task Initialize(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<DbInitializer> logger, IConfiguration config)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
            _config = config;

            // Get admin-credentials from UserManager and secrets.json
            string adminUser = _config["AdminUser:UserId"];
            string adminPassword = _config["adminUser:AdminPassword"];
            var defaultUser = await CreateDefaultUser(_userManager, _logger, adminUser, adminPassword);

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

```


&nbsp;

