[Introduction to Identity on ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity)

Demo af en Web App med lokal opbevaring af password, med explicit policies.

Web siden og klik på **Register** og opret:

**User:** ecr@live.dk
**Password:** P@ssw0rd

Opdater databasen ved at klikke på knappen **Apply Migrations** på websiden eller lav en `update-database`. Refresh siden.

Klik på *Confirm your account* for at simulere Register confirmation via e-mail.

Log in og klik på ```Hello<login-navn>``` for at lave **Manage your account**.


## Configure Identity services
Demo af default policy i Startup.cs
