using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AldeiaParental.Data;
using AldeiaParental.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AldeiaParental.Pages.FindCaregivers
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AldeiaParentalUser> _userManager;
        private readonly SignInManager<AldeiaParentalUser> _signInManager;
        private readonly RoleManager<AldeiaParentalRole> _roleManager;

        private const string _caregiverRole = "Cuidador";
        public IndexModel(ApplicationDbContext context,
                UserManager<AldeiaParentalUser> userManager,
                SignInManager<AldeiaParentalUser> signInManager,
                RoleManager<AldeiaParentalRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        public IList<AldeiaParentalUser> Caregivers { get; set; }
        public IList<ServiceLocation> ServiceLocations { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var careGivers = await _userManager.GetUsersInRoleAsync(_caregiverRole);

            if (careGivers == null)
            {
                return NotFound($"No Caregivers Found'.");
            }
            Caregivers = careGivers
                .Where(u => u.EmailConfirmed)
                .ToList();

            return Page();
        }
    }
}
