using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AldeiaParental.Data;
using AldeiaParental.Models;

namespace AldeiaParental.Pages.ServiceLocations
{
    public class DetailsModel : PageModel
    {
        private readonly AldeiaParental.Data.ApplicationDbContext _context;

        public DetailsModel(AldeiaParental.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public ServiceLocation ServiceLocation { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ServiceLocation = await _context.ServiceLocation
                .Include(s => s.Region).FirstOrDefaultAsync(m => m.Id == id);

            if (ServiceLocation == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
