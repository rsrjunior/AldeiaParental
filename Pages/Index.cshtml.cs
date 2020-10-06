using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AldeiaParental.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace AldeiaParental.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly UserManager<AldeiaParentalUser> _userManager;
        private readonly SignInManager<AldeiaParentalUser> _signInManager;



        public IndexModel(ILogger<IndexModel> logger,
            UserManager<AldeiaParentalUser> userManager,
            SignInManager<AldeiaParentalUser> signInManager)
        {
            _logger = logger; 
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string UserFirstName { get; set; }
        public IList<string> UserRoles { get; set; } 

        [TempData]
        public string StatusMessage { get; set; }

        private async Task LoadAsync(AldeiaParentalUser user)
        {
            UserFirstName = user.FirstName;
            UserRoles = await _userManager.GetRolesAsync(user);

        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            UserFirstName = user.FirstName;
            UserRoles = await _userManager.GetRolesAsync(user);

            //await LoadAsync(user);
            return Page();
        }



    }
}
