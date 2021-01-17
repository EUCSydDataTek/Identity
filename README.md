## 5. Microsoft External Login
[Microsoft Account external login](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/microsoft-logins)

F�r demo: slet og update databasen ved at skrive f�lgende i PMB:
```
drop-database
update-database
```

#### Install�r MicrosoftAccount NuGet pakke
Installer `Microsoft.AspNetCore.Authentication.MicrosoftAccount` NuGet package til projektet.
&nbsp;

#### Opret en app i Microsoft Developer Portal. 

Log ind p� [Azure portal - App registrations](https://go.microsoft.com/fwlink/?linkid=2083908) skole-konto: ecr@eucsyd.dk.

&nbsp;
Bem�rk at man under `Redirect URI` skal �ndre IP og portnummer s� det svarer til aktuel ops�tning.
&nbsp;
![Create app in MS Developer Portal](CreateApp.png)
&nbsp;

![Create Secrets](CreateSecrets.png)
&nbsp;

#### Store the Microsoft client ID and client secret

Application (client) ID: findes under Overview
ClientSecret: findes under Certifcates & secrets.

F�lgende er aktuelt benyttet i denne demo app og skal tilf�jes ved at h�jre klikke p� projektet og v�lge
**Manage User Secret**s: 

Secret Manager:
```json
{
  "Authentication": {
    "Microsoft": {
      "ClientId": "<clientId>",
      "ClientSecret": "<clientSecret>"
    }
  }
}
```
&nbsp;

### Configuration i Startup.cs

Tilf�j f�lgende kode til Configuration():

```c#
services.AddAuthentication().AddMicrosoftAccount(microsoftOptions =>
{
    microsoftOptions.ClientId = Configuration["Authentication:Microsoft:ClientId"];
    microsoftOptions.ClientSecret = Configuration["Authentication:Microsoft:ClientSecret"];
});
```

&nbsp;

### Configuration i Program.cs

Det er vigtigt at sikre sig at der er udf�rt en indledende Migration, s� det ikke er den f�rste bruger der selv skal trigge den:

```c#
public class Program
{
    public static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();
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
``` 

&nbsp;

# Login

N�r der logges ind med Microsoft f�rste gang bliver man pr�senteret for en consent:

![Consent](Consent.png)
&nbsp;

Herefter f�r man mulighed for at forbinde sit Microsoft login med et lokalt login (som f.eks. kan benyttes til at styre roles):
![AssociateMSaccount](AssociateMSaccount.png)
&nbsp;


### Hvad gemmes i databasen?
Her ses de to rows i databasen i AspNetUsers tabellen:

![AspNetUsers](AspNetUsers.png)