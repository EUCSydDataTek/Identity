# 4. Roles Management
[Yogihosting.com](https://www.yogihosting.com/aspnet-core-identity-roles/)

## Oprettelse af Role klasser Role-Service
Der oprettes en folder under *Areas | Identity* kaldet **Models**.
Her oprettes klassen **RoleEdit**:
```csharp
public class RoleEdit
{
    public IdentityRole Role { get; set; }
    public IEnumerable<IdentityUser> Members { get; set; }
    public IEnumerable<IdentityUser> NonMembers { get; set; }
}
```
&nbsp;

Opret ligeledes klassen **RoleModification**:
```csharp
public class RoleModification
{
    [Required]
    public string RoleName { get; set; }
    public string RoleId { get; set; }
    public string[] AddIds { get; set; }
    public string[] DeleteIds { get; set; }
}
```

Tilføj linjen med .AddRoles<> i `Startup.cs`:
```csharp
services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
```
&nbsp;

### Oprettelse af Pages til Roles Management
Der oprettes en folder under *Areas | Identity | Pages* kaldet **Roles**.

Her oprettes 4 Pages i Roles-folderen:
- ListRoles
- Create
- Update
- Delete

#### ListRoles.aspx
```html
@page
@model WebAppLocalStorage.Areas.Identity.Pages.Roles.ListRolesModel
@{
    ViewData["Title"] = "ListRoles";
}

<h1>All Roles</h1>
<a asp-page="Create" class="btn btn-primary my-4">Create a Role</a>

<table class="table table-sm table-bordered table-bordered">
    <tr><th>ID</th><th>Name</th><th>Users</th><th>Update</th><th>Delete</th></tr>
    @foreach (var role in Model.Roles)
    {
        <tr>
            <td>@role.Id</td>
            <td>@role.Name</td>
            <td i-role="@role.Id"></td>
            <td><a class="btn btn-sm btn-primary" asp-page="Update" asp-route-id="@role.Id">Update</a></td>
            <td>
                <form asp-page="Delete" asp-page-handler="Delete" asp-route-id="@role.Id" method="post">
                    <button type="submit" class="btn btn-sm btn-danger">
                        Delete
                    </button>
                </form>
            </td>
        </tr>
    }
</table>
```
&nbsp;

#### ListRoles.aspx.cs
```csharp
public class ListRolesModel : PageModel
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public ListRolesModel(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public IEnumerable<IdentityRole> Roles { get; set; }

    public void OnGet() => Roles = _roleManager.Roles;
}
```
&nbsp;

#### Create.aspx
```html
@page
@model WebAppLocalStorage.Areas.Identity.Pages.Roles.CreateModel
@{
    ViewData["Title"] = "Create";
}

<h1 class="bg-info text-white">Create a Role</h1>
<a asp-action="Index" class="btn btn-secondary my-4">Back</a>
<div asp-validation-summary="All" class="text-danger"></div>

<form asp-action="Create" method="post">
    <div class="form-group">
        <label for="name">Name:</label>
        <input name="name" class="form-control" />
    </div>
    <button type="submit" class="btn btn-primary">Create</button>
</form>
```
&nbsp;

#### Create.aspx.cs
```csharp
public class CreateModel : PageModel
{
    private readonly RoleManager<IdentityRole> _roleManager;
    public CreateModel(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public void OnGet()
    { }

    public async Task<IActionResult> OnPost(string name)
    {
        if (ModelState.IsValid)
        {
            IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
            if (result.Succeeded)
                return RedirectToPage("./ListRoles");
            else
                Errors(result);
        }
        return Page();
    }

    private void Errors(IdentityResult result)
    {
        foreach (IdentityError error in result.Errors)
            ModelState.AddModelError("", error.Description);
    }
}
```
&nbsp;

#### Update.cshtml
```html
@page
@model WebAppLocalStorage.Areas.Identity.Pages.Roles.UpdateModel
@{
    ViewData["Title"] = "Update";
}

<h1>Update Role</h1>
<a asp-page="ListRoles" class="btn btn-outline-primary my-4">Back</a>
<div asp-validation-summary="All" class="text-danger"></div>

<form method="post">
    <input type="hidden" name="roleName" value="@Model.RoleEdit.Role.Name" />
    <input type="hidden" name="roleId" value="@Model.RoleEdit.Role.Id" />

    <h2>Add To @Model.RoleEdit.Role.Name</h2>
    <table class="table table-bordered table-sm">
        @if (Model.RoleEdit.NonMembers.Count() == 0)
        {
            <tr><td colspan="2">All Users Are Members</td></tr>
        }
        else
        {
            @foreach (var user in Model.RoleEdit.NonMembers)
            {
                <tr>
                    <td>@user.UserName</td>
                    <td>
                        <input type="checkbox" name="AddIds" value="@user.Id">
                    </td>
                </tr>
            }
        }
    </table>

    <h2>Remove From @Model.RoleEdit.Role.Name</h2>
    <table class="table table-bordered table-sm">
        @if (Model.RoleEdit.Members.Count() == 0)
        {
            <tr><td colspan="2">No Users Are Members</td></tr>
        }
        else
        {
            @foreach (var user in Model.RoleEdit.Members)
            {
                <tr>
                    <td>@user.UserName</td>
                    <td>
                        <input type="checkbox" name="DeleteIds" value="@user.Id">
                    </td>
                </tr>
            }
        }
    </table>
    <button type="submit" class="btn btn-primary">Save</button>
</form>
```
&nbsp;

#### Update.cshtml.cs
```csharp
public class UpdateModel : PageModel
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<IdentityUser> _userManager;

    public UpdateModel(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    [BindProperty]
    public RoleEdit RoleEdit { get; set; }

    [BindProperty]
    public RoleModification RoleModification { get; set; }

    public async Task OnGet(string id)
    {
        IdentityRole role = await _roleManager.FindByIdAsync(id);
        List<IdentityUser> members = new List<IdentityUser>();
        List<IdentityUser> nonMembers = new List<IdentityUser>();
        foreach (IdentityUser user in _userManager.Users)
        {
            var list = await _userManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;
            list.Add(user);
        }
        RoleEdit = new RoleEdit
        {
            Role = role,
            Members = members,
            NonMembers = nonMembers
        };
    }

    public async Task<IActionResult> OnPost()
    {
        IdentityResult result;
        if (ModelState.IsValid)
        {
            foreach (string userId in RoleModification.AddIds ?? new string[] { })
            {
                IdentityUser user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    result = await _userManager.AddToRoleAsync(user, RoleModification.RoleName);
                    if (!result.Succeeded)
                        Errors(result);
                }
            }
            foreach (string userId in RoleModification.DeleteIds ?? new string[] { })
            {
                IdentityUser user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    result = await _userManager.RemoveFromRoleAsync(user, RoleModification.RoleName);
                    if (!result.Succeeded)
                        Errors(result);
                }
            }
        }

        if (ModelState.IsValid)
            return RedirectToPage("ListRoles");
        else
            return Page();
    }

    private void Errors(IdentityResult result)
    {
        foreach (IdentityError error in result.Errors)
            ModelState.AddModelError("", error.Description);
    }
}
```
&nbsp;

#### Delete.cshtml
```html
@page
@model WebAppLocalStorage.Areas.Identity.Pages.Roles.DeleteModel
@{
    ViewData["Title"] = "Delete";
}

<h1>Delete</h1>
```
&nbsp;

#### Delete.cshtml.cs
```c#
public class DeleteModel : PageModel
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public DeleteModel(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostDeleteAsync(string id)
    {
        IdentityRole role = await _roleManager.FindByIdAsync(id);
        if (role != null)
        {
            IdentityResult result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
                return RedirectToPage("./ListRoles");
            else
                Errors(result);
        }
        else
            ModelState.AddModelError("", "No role found");
        return RedirectToPage("./ListRoles", _roleManager.Roles);
    }

    private void Errors(IdentityResult result)
    {
        foreach (IdentityError error in result.Errors)
            ModelState.AddModelError("", error.Description);
    }
}
```

&nbsp;
#### Oprettelse af TagHelper
Vi ønsker at ListRoles sidden kan vise en oversigt over hvilke brugere, der har hvilke roller. 
Det kræver en TagHelper, kan omsætte et ID til brugerens Username:

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


### Sæt Authorization på klassen
Sæt `[Authorize(Roles="Manager")]`  på siden Privacy.
Læs mere her [Role-based authorization in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/authorization/roles?view=aspnetcore-3.1)

https://stackoverflow.com/questions/50785009/how-to-seed-an-admin-user-in-ef-core-2-1-0