using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AldeiaParental.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using AldeiaParental.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace AldeiaParental.Areas.Identity.Pages.Account.Manage
{
    public class DeletePersonalDataModel : PageModel
    {
        private readonly UserManager<AldeiaParentalUser> _userManager;
        private readonly SignInManager<AldeiaParentalUser> _signInManager;
        private readonly ILogger<DeletePersonalDataModel> _logger;
        private readonly ApplicationDbContext _context;

        public DeletePersonalDataModel(
            UserManager<AldeiaParentalUser> userManager,
            SignInManager<AldeiaParentalUser> signInManager,
            ILogger<DeletePersonalDataModel> logger,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public bool RequirePassword { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            RequirePassword = await _userManager.HasPasswordAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            RequirePassword = await _userManager.HasPasswordAsync(user);
            if (RequirePassword)
            {
                if (!await _userManager.CheckPasswordAsync(user, Input.Password))
                {
                    ModelState.AddModelError(string.Empty, "Senha Incorreta");
                    return Page();
                }
            }
            bool ratedServices = _context.Service.Any(s => (s.CustomerId == user.Id || s.CaregiverId == user.Id) && s.Rate.HasValue);

            if (ratedServices)
            {
                ModelState.AddModelError(string.Empty, "Impossível Excluir: existem atendimentos registrados.");
                return Page();
            }



            IList<Service> ser = _context.Service
            .Where(s => (s.CustomerId == user.Id || s.CaregiverId == user.Id))
            .ToList();
            _context.Service.RemoveRange(ser);

            var result = await _userManager.DeleteAsync(user);
            var userId = await _userManager.GetUserIdAsync(user);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Unexpected error occurred deleting user with ID '{userId}'.");
            }

            await _signInManager.SignOutAsync();

            _logger.LogInformation("User with ID '{UserId}' deleted themselves.", userId);

            return Redirect("~/");
        }
    }
}
