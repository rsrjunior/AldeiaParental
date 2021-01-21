using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AldeiaParental.Data;
using AldeiaParental.Models;
using Microsoft.AspNetCore.Identity;

namespace AldeiaParental.Pages.FindCaregivers
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AldeiaParentalUser> _userManager;

        public DetailsModel(ApplicationDbContext context,
            UserManager<AldeiaParentalUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public AldeiaParentalUser Caregiver { get; set; }
        public bool VerifiedDoc { get; set; }
        public IList<ServiceLocation> ServiceLocations { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Caregiver = await _userManager.FindByIdAsync(id);

            if (Caregiver == null)
            {
                return NotFound();
            }

            VerifiedDoc = _context.PersonalDocument
                .Where(d => d.UserId == Caregiver.Id)
                .Any(d => d.Valid.HasValue && d.Valid.Value);

            ServiceLocations = await _context.ServiceLocation
                .Where(s => s.UserId == Caregiver.Id)
                .Include(s => s.Region)
                .ToListAsync();

            return Page();
        }
    }
}
