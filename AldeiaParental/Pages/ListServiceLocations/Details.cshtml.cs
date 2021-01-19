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

namespace AldeiaParental.Pages_ListServiceLocations
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public DetailsModel(ApplicationDbContext context)
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
