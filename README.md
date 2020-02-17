## 5. Microsoft External Login
[Microsoft Account external login](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/microsoft-logins)

Før demo: slet og update databasen ved at skrive følgende i PMB:
```
drop-database
update-database
```

#### Installér MicrosoftAccount NuGet pakke
Installer `Microsoft.AspNetCore.Authentication.MicrosoftAccount` NuGet package til projektet.
&nbsp;

#### Opret en app i Microsoft Developer Portal. 

Log ind på [Azure portal - App registrations](https://go.microsoft.com/fwlink/?linkid=2083908) skole-konto: ecr@eucsyd.dk.

&nbsp;
Bemærk at man under `Redirect URI` skal ændre IP og portnummer så det svarer til aktuel opsætning.
&nbsp;
![Create app in MS Developer Portal](CreateApp.png)
&nbsp;

![Create Secrets](CreateSecrets.png)
&nbsp;

#### Store the Microsoft client ID and client secret

Application (client) ID: findes under Overview
ClientSecret: findes under Certifcates & secrets.

Følgende er aktuelt benyttet i denne demo app og skal tilføjes ved at højre klikke på projektet og vælge
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

Tilføj følgende kode til Configuration():

```c#
services.AddAuthentication().AddMicrosoftAccount(microsoftOptions =>
{
    microsoftOptions.ClientId = Configuration["Authentication:Microsoft:ClientId"];
    microsoftOptions.ClientSecret = Configuration["Authentication:Microsoft:ClientSecret"];
});
```
&nbsp;
### Login

Når der logges ind med Microsoft første gang bliver man præsenteret for en consent:

![Consent](Consent.png)
&nbsp;

Herefter får man mulighed for at forbinde sit Microsoft login med et lokalt login (som f.eks. kan benyttes til at styre roles):
![AssociateMSaccount](AssociateMSaccount.png)
&nbsp;


### Hvad gemmes i databasen?
Her ses de to rows i databasen i AspNetUsers tabellen:

![AspNetUsers](AspNetUsers.png)