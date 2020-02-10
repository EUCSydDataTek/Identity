[Introduction to Identity on ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-3.1&tabs=visual-studio#create-a-web-app-with-authentication)

## Demo af en Web App med lokal opbevaring af password.

#### Opret projektet med Security = Individual User Accounts

#### Tilf�j Migration
Opdater databasen ved at klikke p� knappen **Apply Migrations** p� websiden eller lav en `update-database`. Refresh siden.

#### Test og Login
Web siden og klik p� **Register** og opret:

**User:** ecr@live.dk
**Password:** P@ssw0rd

Klik p� *Confirm your account* for at simulere Register confirmation via e-mail.

Log in og klik p� ```Hello<login-navn>``` for at lave **Manage your account**.


#### Test Identiy
S�t `[Authorize]` p� klassen for Privacy klassen og se at man kun f�r adgang hvis man er authentikeret.


#### Unders�g Identity databasen


