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
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace AldeiaParental.Areas_Identity_Pages_Account_Manage_PersonalDocuments
{
    public class DeleteModel : PageModel
    {
        private readonly AldeiaParental.Data.ApplicationDbContext _context;
        private readonly UserManager<AldeiaParentalUser> _userManager;
        private IWebHostEnvironment _environment;
        public DeleteModel(AldeiaParental.Data.ApplicationDbContext context,
        UserManager<AldeiaParentalUser> userManager,
        IWebHostEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _environment = environment;
        }

        [BindProperty]
        public PersonalDocument PersonalDocument { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PersonalDocument = await _context.PersonalDocument
                .Include(p => p.User).FirstOrDefaultAsync(m => m.Id == id);

            if (PersonalDocument == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PersonalDocument = await _context.PersonalDocument.FindAsync(id);

            if (PersonalDocument != null)
            {
                if (PersonalDocument.UserId==_userManager.GetUserId(User) &&
                    !(PersonalDocument.Valid??false))
                {
                     if(!String.IsNullOrEmpty(PersonalDocument.FilePath) &&
                        System.IO.File.Exists(Path.Combine(_environment.ContentRootPath, "uploads", PersonalDocument.FilePath)))
                    {
                        System.IO.File.Delete(Path.Combine(_environment.ContentRootPath, "uploads", PersonalDocument.FilePath));
                    }
                    _context.PersonalDocument.Remove(PersonalDocument);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
