using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TrickingLibirary.Api.Pages.Account
{
    public class Register : PageModel
    {
        [BindProperty]
        public RegisterForm Form { get; set; }
        public void OnGet(string returnUrl)
        {
            Form = new RegisterForm { ReturnUrl = returnUrl };
        }
        public async Task<IActionResult> OnPostAsync([FromServices] UserManager<IdentityUser> userManager,
            [FromServices] SignInManager<IdentityUser> signInManager)
        {
            if (ModelState.IsValid is false) return Page();
            var user = new IdentityUser(Form.Username) { Email = Form.Email };
            var identityResult = await userManager.CreateAsync(user, Form.Password);
            if (identityResult.Succeeded)
            {
                await signInManager.SignInAsync(user, true);
                return Redirect(Form.ReturnUrl);
            }
            return Page();
        }
        public class RegisterForm
        {
            public string ReturnUrl { get; set; }
            [DisplayName("E-Mail")]
            [Required]
            [DataType(DataType.EmailAddress)]
            public string Email { get; set; }
            [Required]
            public string Username { get; set; }
            [DisplayName("Password")]
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
            [DisplayName("Confirm Password")]
            [Required]
            [DataType(DataType.Password)]
            [Compare(nameof(Password))]
            public string ConfirmPassword { get; set; }
        }
    }
}
