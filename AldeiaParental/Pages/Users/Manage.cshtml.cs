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

namespace AldeiaParental.Pages_Users
{
    public class ManageModel : PageModel
    {
        private readonly AldeiaParental.Data.ApplicationDbContext _context;

        public ManageModel(AldeiaParental.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public AldeiaParentalUser AldeiaParentalUser { get; set; }
        [BindProperty]
        public DateTime? LockoutEnd { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AldeiaParentalUser = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (AldeiaParentalUser.LockoutEnd.HasValue)
                LockoutEnd = AldeiaParentalUser.LockoutEnd.Value.DateTime;
            if (AldeiaParentalUser == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {            
            if (LockoutEnd.HasValue)
            {
                AldeiaParentalUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == AldeiaParentalUser.Id);
                AldeiaParentalUser.LockoutEnd = new DateTimeOffset(LockoutEnd.Value);
                ModelState.Clear();
                if (!TryValidateModel(AldeiaParentalUser))
                {
                    return Page();
                }
                _context.Attach(AldeiaParentalUser).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AldeiaParentalUserExists(AldeiaParentalUser.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

            }

            return RedirectToPage("./Index");
        }

        private bool AldeiaParentalUserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
