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
using Microsoft.AspNetCore.Identity;

namespace AldeiaParental.Pages.ServiceLocations
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AldeiaParentalUser> _userManager;


        public EditModel(ApplicationDbContext context,
            UserManager<AldeiaParentalUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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
                .Where(s => s.UserId == _userManager.GetUserId(User))
                .Include(s => s.Region)
                .Include(s => s.User).FirstOrDefaultAsync(m => m.Id == id);

            if (ServiceLocation == null)
            {
                return NotFound();
            }

            ViewData["Region"] = new SelectList(_context.Region, "Id", "Name");

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            //ensure that only updates on current auth user
            ServiceLocation.UserId = _userManager.GetUserId(User);

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
