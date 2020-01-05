## 5. Microsoft External Login
[Microsoft Account external login](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/microsoft-logins)

### SSO Logout
I Logout.aspx.cs i metoden OnPost() skal følgende tilføjes:

` return Redirect("https://login.microsoftonline.com/common/oauth2/v2.0/logout?post_logout_redirect_uri=https://localhost:44377/identity/account/logout");`

Læs evt. mere her: [Send a sign-out request](https://docs.microsoft.com/en-us/azure/active-directory/develop/v2-protocols-oidc#send-a-sign-out-request)