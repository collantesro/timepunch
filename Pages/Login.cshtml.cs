using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace timepunch.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public LoginData loginData { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                bool isValid = true;
                User dbUser = DataAccess.GetUserByEmail(loginData.Email);
                if (dbUser == null)
                {
                    isValid = false;
                }
                else
                {
                    isValid = PasswordUtils.checkPassword(dbUser.PasswordHash, dbUser.Salt, loginData.Password);
                }

                if (!isValid)
                {
                    ModelState.AddModelError("", "Email or Password is invalid");
                    return Page();
                }

                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, dbUser.Email));
                identity.AddClaim(new Claim(ClaimTypes.Name, dbUser.Email));
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties { IsPersistent = true, ExpiresUtc = DateTime.Now.AddHours(2) });
                return RedirectToPage("Index");
            }
            else
            {
                ModelState.AddModelError("", "Email or Password fields are blank");
                return Page();
            }
        }

        public class LoginData
        {

            [Required, DataType(DataType.EmailAddress)]
            public string Email { get; set; }

            [Required, DataType(DataType.Password)]
            public string Password { get; set; }
        }
    }
}