using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
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
    public class ExternalLoginModel : PageModel
    {
        private readonly SignInManager<AldeiaParentalUser> _signInManager;
        private readonly UserManager<AldeiaParentalUser> _userManager;
        private readonly RoleManager<AldeiaParentalRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ExternalLoginModel> _logger;
        private const string _customerRole = "Cliente";
        private const string _caregiverRole = "Cuidador";
        public ExternalLoginModel(
            SignInManager<AldeiaParentalUser> signInManager,
            UserManager<AldeiaParentalUser> userManager,
            RoleManager<AldeiaParentalRole> roleManager,
            ILogger<ExternalLoginModel> logger,
            IEmailSender emailSender)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ProviderDisplayName { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "{0} Obrigatório.")]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }
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

           
        }

        public IActionResult OnGetAsync()
        {
            return RedirectToPage("./Login");
        }

        public IActionResult OnPost(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToPage("./Login", new {ReturnUrl = returnUrl });
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor : true);
            if (result.Succeeded)
            {
                _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
                return LocalRedirect(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return RedirectToPage("./Lockout");
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ReturnUrl = returnUrl;
                ProviderDisplayName = info.ProviderDisplayName;
                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                {
                    Input = new InputModel
                    {
                        Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                        FirstName = info.Principal.FindFirstValue(ClaimTypes.Name),
                        LastName = info.Principal.FindFirstValue(ClaimTypes.Surname)
                    };
                }
                return Page();
            }
        }

        public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            // Get the information about the user from the external login provider
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information during confirmation.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            if (ModelState.IsValid)
            {
                var user = new AldeiaParentalUser { 
                    UserName = Input.Email,
                    Email = Input.Email, 
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                    RegistrationDate = DateTime.UtcNow };

                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);

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

                        var userId = await _userManager.GetUserIdAsync(user);
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId = userId, code = code },
                            protocol: Request.Scheme);

                        await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                        // If account confirmation is required, we need to show the link if we don't have a real email sender
                        if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            return RedirectToPage("./RegisterConfirmation", new { Email = Input.Email });
                        }

                        await _signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);

                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            ProviderDisplayName = info.ProviderDisplayName;
            ReturnUrl = returnUrl;
            return Page();
        }
    }
}
