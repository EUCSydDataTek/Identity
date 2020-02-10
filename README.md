[Introduction to Identity on ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-3.1&tabs=visual-studio#create-a-web-app-with-authentication)

## Demo af en Web App med lokal opbevaring af password.

#### Opret projektet med Security = Individual User Accounts

#### Tilføj Migration
Opdater databasen ved at klikke på knappen **Apply Migrations** på websiden eller lav en `update-database`. Refresh siden.

#### Test og Login
Web siden og klik på **Register** og opret:

**User:** ecr@live.dk
**Password:** P@ssw0rd

Klik på *Confirm your account* for at simulere Register confirmation via e-mail.

Log in og klik på ```Hello<login-navn>``` for at lave **Manage your account**.


#### Test Identiy
Sæt `[Authorize]` på klassen for Privacy klassen og se at man kun får adgang hvis man er authentikeret.


#### Undersøg Identity databasen


