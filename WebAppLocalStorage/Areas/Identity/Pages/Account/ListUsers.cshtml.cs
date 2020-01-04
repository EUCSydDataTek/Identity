using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace WebAppLocalStorage.Areas.Identity.Pages.Account
{
    public class ListUsersModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public ListUsersModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public IEnumerable<IdentityUser> Users { get; set; }

        public void OnGet()
        {
            Users = _userManager.Users;
        }
    }
}
