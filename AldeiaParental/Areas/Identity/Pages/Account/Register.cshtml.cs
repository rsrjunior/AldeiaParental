using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using AldeiaParental.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace AldeiaParental.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<AldeiaParentalUser> _signInManager;
        private readonly UserManager<AldeiaParentalUser> _userManager;
        private readonly RoleManager<AldeiaParentalRole> _roleManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        private const string _customerRole = "Cliente";
        private const string _caregiverRole = "Cuidador";
        public RegisterModel(
            UserManager<AldeiaParentalUser> userManager,
            RoleManager<AldeiaParentalRole> roleManager,
            SignInManager<AldeiaParentalUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "{0} Obrigatório.")]
            [StringLength(30, ErrorMessage = "{0} deve conter no máximo {1} caracteres.")]
            [Display(Name = "Nome")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "{0} Obrigatório.")]
            [StringLength(70, ErrorMessage = "{0} deve conter no máximo {1} caracteres.")]
            [Display(Name = "Sobrenome")]
            public string LastName { get; set; }

            [Display(Name = "Cliente")]
            public bool Customer { get; set; }
            [Display(Name = "Cuidador")]
            public bool Caregiver { get; set; }

            [Required(ErrorMessage = "{0} Obrigatório.")]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "{0} Obrigatório.")]
            [StringLength(100, ErrorMessage = "A {0} deve conter entre {2} e {1} caracteres.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Senha")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirme a senha")]
            [Compare("Password", ErrorMessage = "A confirmação e a senha não conferem.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new AldeiaParentalUser 
                { UserName = Input.Email,
                    Email = Input.Email, 
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                    RegistrationDate = DateTime.UtcNow };

                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    //try to set roles

                    #region set roles
                    if (Input.Customer)
                    {

                        bool customerRoleFound = await _roleManager.RoleExistsAsync(_customerRole);
                        if (!customerRoleFound) {
                            _logger.LogWarning("Could not set role for " + user.UserName
                            + " The role " + _customerRole + "does not exist");
                        }
                        else
                        {
                            try
                            {
                                await _userManager.AddToRoleAsync(user, _customerRole);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogWarning("Could not set "+ _customerRole + " role for " + user.UserName
                            + " "+ex.Message);
                            }
                        }
                    }

                    if (Input.Caregiver)
                    {

                        bool caregiverRoleFound = await _roleManager.RoleExistsAsync(_caregiverRole);
                        if (!caregiverRoleFound)
                        {
                            _logger.LogWarning("Could not set role for " + user.UserName
                            + " The role " + _caregiverRole + "does not exist");
                        }
                        else
                        {
                            try
                            {
                                await _userManager.AddToRoleAsync(user, _caregiverRole);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogWarning("Could not set " + _caregiverRole + " role for " + user.UserName
                            + " " + ex.Message);
                            }
                        }
                    }
                    #endregion

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code, returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
