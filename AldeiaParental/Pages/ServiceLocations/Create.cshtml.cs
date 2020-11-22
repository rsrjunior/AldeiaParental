using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using AldeiaParental.Data;
using AldeiaParental.Models;
using Microsoft.AspNetCore.Identity;

namespace AldeiaParental.Pages.ServiceLocations
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AldeiaParentalUser> _userManager;

        public CreateModel(ApplicationDbContext context,
            UserManager<AldeiaParentalUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult OnGet()
        {
        ViewData["Region"] = new SelectList(_context.Region, "Id", "Name");
         
            return Page();
        }

        [BindProperty]
        public ServiceLocation ServiceLocation { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            //setar o userId para o Id do Usuario da seção.
            ServiceLocation.UserId = _userManager.GetUserId(User);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.ServiceLocation.Add(ServiceLocation);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
