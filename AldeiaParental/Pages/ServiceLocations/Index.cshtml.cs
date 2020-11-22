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
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AldeiaParentalUser> _userManager;

        public IndexModel(ApplicationDbContext context,
            UserManager<AldeiaParentalUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<ServiceLocation> ServiceLocation { get;set; }

        public async Task OnGetAsync()
        {
            string userId = _userManager.GetUserId(User);
         
            ServiceLocation = await _context.ServiceLocation
                .Where(s => s.UserId == userId)
                .Include(s => s.Region)
                .Include(s => s.User).ToListAsync();
        }
    }
}
