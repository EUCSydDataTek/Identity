## Konfiguration af Identiy Services

[Introduction to Identity on ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-3.1&tabs=visual-studio#configure-identity-services)

Viser default properties for Password, Lockout, User og Cookie indstillinger.

[Læs mere hos Microsoft Docs](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity-configuration?view=aspnetcore-2.2#password)


## Udvidet policies

### Creating Custom Password Policy
[Yogihosting.com](https://www.yogihosting.com/aspnet-core-identity-username-email-password-policy/)

Man kan lave sin helt egen Password Policy ved at nedarve fra ```PasswordValidator```:

```c#
public class CustomPasswordPolicy : PasswordValidator<IdentityUser>
{
    public override async Task<IdentityResult> ValidateAsync(UserManager<IdentityUser> manager, IdentityUser user, string password)
    {
        // Validates according to rules ind ConfigureServices()
        IdentityResult result = await base.ValidateAsync(manager, user, password);

        // If success, then proceed with a custom check
        List<IdentityError> errors = result.Succeeded ? new List<IdentityError>() : result.Errors.ToList();

        if (password.ToLower().Contains(user.UserName.ToLower()))
        {
            errors.Add(new IdentityError
            {
                Description = "Password cannot contain username"
            });
        }
        if (password.Contains("123"))
        {
            errors.Add(new IdentityError
            {
                Description = "Password cannot contain 123 numeric sequence"
            });
        }
        return errors.Count == 0 ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray());
    }
}
```

Password validation funktionen er defineret i `IPasswordValidator` og skal derfor registreres som service i ConfigureServices():

```c#
services.AddTransient<IPasswordValidator<IdentityUser>, CustomPasswordPolicy>();
```

### Email Policy in Identity

På samme måde som vi lavede Password policies, kan man også benytte policies for Email (username)  vha. ```IdentiyOptions.User``` property.
Tilføj følgende til ConfigureServices():
```c#
opts.User.RequireUniqueEmail = true;
opts.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyz";
```

### Creating Custom Username and Email Policy

Også her kan man lave custom policies ved at nedarve fra ```UserValidator<T>```:

```c#
public class CustomUsernameEmailPolicy : UserValidator<IdentityUser>
{
    public override async Task<IdentityResult> ValidateAsync(UserManager<IdentityUser> manager, IdentityUser user)
    {
        IdentityResult result = await base.ValidateAsync(manager, user);
        List<IdentityError> errors = result.Succeeded ? new List<IdentityError>() : result.Errors.ToList();

        if (!user.Email.ToLower().EndsWith("@eucsyd.dk"))
        {
            errors.Add(new IdentityError
            {
                Description = "Only eucsyd.dk email addresses are allowed"
            });
        }
        return errors.Count == 0 ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray());
    }
}
```

Denne klasse skal også registreres som en service i ConfigureServices():
```c#
services.AddTransient<IUserValidator<IdentityUser>, CustomUsernameEmailPolicy>();
```

## Applying Password, Username and Email Policies when Updating a User Account

Desværre bliver disse policies ikke benyttet når man opdaterer en allerede oprettet user.
