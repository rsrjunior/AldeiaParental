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

namespace AldeiaParental.Pages.ServiceLocations
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

        public ServiceLocation ServiceLocation { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ServiceLocation = await _context.ServiceLocation
                .Where(s=>s.UserId==_userManager.GetUserId(User))
                .Include(s => s.Region)
                .Include(s => s.User).FirstOrDefaultAsync(m => m.Id == id);

            if (ServiceLocation == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
