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

namespace AldeiaParental.Areas_Identity_Pages_Account_Manage_PersonalDocuments
{
    public class CreateModel : PageModel
    {
        private readonly AldeiaParental.Data.ApplicationDbContext _context;
        private readonly UserManager<AldeiaParentalUser> _userManager;

        public CreateModel(AldeiaParental.Data.ApplicationDbContext context,
         UserManager<AldeiaParentalUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public PersonalDocument PersonalDocument { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            //setar o userId para o Id do Usuario da seção.
            PersonalDocument.UserId = _userManager.GetUserId(User);
            
            this.ModelState.Clear();
            if (!TryValidateModel(PersonalDocument))
            {
                return Page();
            }
            _context.PersonalDocument.Add(PersonalDocument);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
