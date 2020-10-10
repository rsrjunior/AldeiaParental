using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AldeiaParental.Data;
using AldeiaParental.Models;

namespace AldeiaParental.Pages.ServiceLocations
{
    public class EditModel : PageModel
    {
        private readonly AldeiaParental.Data.ApplicationDbContext _context;

        public EditModel(AldeiaParental.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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
           ViewData["RegionId"] = new SelectList(_context.Region, "Id", "Id");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(ServiceLocation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceLocationExists(ServiceLocation.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ServiceLocationExists(int id)
        {
            return _context.ServiceLocation.Any(e => e.Id == id);
        }
    }
}
