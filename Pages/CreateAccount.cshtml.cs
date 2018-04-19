using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace timepunch.Pages
{
    public class CreateAccountModel : PageModel
    {
        [BindProperty]
        public NewUserData newUserData { get; set; }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                var badPassword = false;
                if (newUserData.Password.Length < 6)
                {
                    ModelState.AddModelError("", "Password is less than 6 characters.");
                    badPassword = true;
                }
                if (newUserData.Password != newUserData.ConfirmPassword)
                {
                    ModelState.AddModelError("", "Password and Confirm Password did not match.");
                    badPassword = true;
                }

                if (badPassword)
                    return Page();

                newUserData.Email = newUserData.Email.ToLowerInvariant();
                var exists = DataAccess.GetUserByEmail(newUserData.Email) != null;
                if (exists)
                {
                    ModelState.AddModelError("", "That email address has already been registered.");
                    return Page();
                }

                // User doesn't already exist, password length >= 6, password has been typed correctly twice.

                var salt = PasswordUtils.generateSalt();
                var hash = PasswordUtils.passwordEncrypt(salt, newUserData.Password);

                var user = new User()
                {
                    Name = newUserData.Name,
                    Email = newUserData.Email,
                    Salt = salt,
                    PasswordHash = hash
                };

                DataAccess.InsertUser(user);
                // If this succeeded, send them to Index, which redirects them to Login
                return RedirectToPage("/Index");
            }
            else
            {
                ModelState.AddModelError("", "Required data is missing.");
                return Page();
            }
        }

        public class NewUserData
        {
            [Required]
            public string Name { get; set; }

            [Required, DataType(DataType.EmailAddress)]
            public string Email { get; set; }

            [Required, DataType(DataType.Password)]
            public string Password { get; set; }

            [Required, DataType(DataType.Password)]
            public string ConfirmPassword { get; set; }

        }
    }
}
