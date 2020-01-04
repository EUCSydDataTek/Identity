# 4. Roles
[ref.](https://www.yogihosting.com/aspnet-core-identity-roles/)

## Creating and Deleting Roles in Identity
Der oprettes en folder under *Areas | Identity | Pages* kaldet **Roles**.

Her oprettes 4 Pages:
- Index
- Create
- Update
- Delete

Der oprettes en folder under *Areas | Identity* kaldet **Models**.
Her oprettes klassen **RoleEdit**:
```cs
public class RoleEdit
{
    public IdentityRole Role { get; set; }
    public IEnumerable<IdentityUser> Members { get; set; }
    public IEnumerable<IdentityUser> NonMembers { get; set; }
}
```

Opret ligeledes klassen **RoleModification**:
```c#
public class RoleModification
{
    [Required]
    public string RoleName { get; set; }
    public string RoleId { get; set; }
    public string[] AddIds { get; set; }
    public string[] DeleteIds { get; set; }
}
```

Tilføj den markerede linje i Startup.cs:
```c#
services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    **.AddRoles<IdentityRole>()**
    .AddEntityFrameworkStores<ApplicationDbContext>();
```

Sæt `[Authorize(Roles="Manager")]`  på siden Privacy.
Læs mere her [Role-based authorization in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/authorization/roles?view=aspnetcore-3.1)


Lav en folder og en klasse i roden af web app'en: `TagHelpers/RoleUsersTH.cs`:
```c#
[HtmlTargetElement("td", Attributes = "i-role")]
public class RoleUsersTH : TagHelper
{
    private UserManager<IdentityUser> userManager;
    private RoleManager<IdentityRole> roleManager;

    public RoleUsersTH(UserManager<IdentityUser> usermgr, RoleManager<IdentityRole> rolemgr)
    {
        userManager = usermgr;
        roleManager = rolemgr;
    }

    [HtmlAttributeName("i-role")]
    public string Role { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        List<string> names = new List<string>();
        IdentityRole role = await roleManager.FindByIdAsync(Role);
        if (role != null)
        {
            foreach (var user in userManager.Users)
            {
                if (user != null && await userManager.IsInRoleAsync(user, role.Name))
                    names.Add(user.UserName);
            }
        }
        output.Content.SetContent(names.Count == 0 ? "No Users" : string.Join(", ", names));
    }
}
```

Tilføj følgende til `Areas | |Identiy | Pages | _ViewImports`: `@addTagHelper WebAppLocalStorage.TagHelpers.*, WebAppLocalStorage`


## Get all Roles in Identity


## Creating a New Role


## Deleting a Role


## Adding and Removing Users from Roles in Identity


## Roles for Authentication
