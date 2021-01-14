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

namespace AldeiaParental.Pages_CustomerServices
{
    public class EditModel : PageModel
    {
        private readonly AldeiaParental.Data.ApplicationDbContext _context;
        private readonly UserManager<AldeiaParentalUser> _userManager;

        public EditModel(AldeiaParental.Data.ApplicationDbContext context,
            UserManager<AldeiaParentalUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Service Service { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Service = await _context.Service
                .Where(s => s.CustomerId == _userManager.GetUserId(User))
                .Include(s => s.Caregiver)
                .Include(s => s.Customer).FirstOrDefaultAsync(s => s.Id == id);
            
            if (Service == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            //ensure that only updates on current auth user and the users for this service
            Service tmp = _context.Service.AsNoTracking().FirstOrDefault(s => s.Id == Service.Id);
            if (!(ModelState.IsValid &&
                tmp.CustomerId == Service.CustomerId &&
                Service.CustomerId == _userManager.GetUserId(User) &&
                tmp.CaregiverId == Service.CaregiverId))
            {
                return Page();
            }

            _context.Attach(Service).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceExists(Service.Id))
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

        private bool ServiceExists(int id)
        {
            return _context.Service.Any(e => e.Id == id);
        }
    }
}
