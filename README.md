## 5. Microsoft External Login
[Microsoft Account external login](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/microsoft-logins)

#### Install�r MicrosoftAccount NuGet pakke
Installer `Microsoft.AspNetCore.Authentication.MicrosoftAccount` NuGet package til projektet.


#### Opret en app i Microsoft Developer Portal. 

Log ind p� [Azure portal - App registrations](https://go.microsoft.com/fwlink/?linkid=2083908) skole-konto: ecr@eucsyd.dk.

---
Bem�rk at man skal v�lge 
![Create app in MS Developer Portal](CreateApp.png)

![Create Secrets](CreateSecrets.png)

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


#### Configuration i Startup.cs

Tilf�j f�lgende kode til Configuration():

```c#
services.AddAuthentication().AddMicrosoftAccount(microsoftOptions =>
{
    microsoftOptions.ClientId = Configuration["Authentication:Microsoft:ClientId"];
    microsoftOptions.ClientSecret = Configuration["Authentication:Microsoft:ClientSecret"];
});
```

#### Login
N�r der logges ind med Microsoft f�rste gang bliver man pr�senteret for en consent:
![Consent](Consent.png)
---
![AssociateMSaccount](AssociateMSaccount.png)

---

![RegisterConfirmation](RegisterConfirmation.png)


#### Hvad gemmes i databasen?
Her ses de to rows i databasen i AspNetUsers tabellen:

![AspNetUsers](AspNetUsers.png)