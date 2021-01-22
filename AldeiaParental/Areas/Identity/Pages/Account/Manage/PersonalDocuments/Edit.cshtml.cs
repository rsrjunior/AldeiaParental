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
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace AldeiaParental.Areas_Identity_Pages_Account_Manage_PersonalDocuments
{
    public class EditModel : PageModel
    {
        private readonly AldeiaParental.Data.ApplicationDbContext _context;
        private readonly UserManager<AldeiaParentalUser> _userManager;
        private IWebHostEnvironment _environment;
        public EditModel(AldeiaParental.Data.ApplicationDbContext context,
        UserManager<AldeiaParentalUser> userManager,
        IWebHostEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _environment = environment;
        }

        [BindProperty]
        public PersonalDocument PersonalDocument { get; set; }
        
        [BindProperty]
        public IFormFile Upload { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PersonalDocument = await _context.PersonalDocument
                .Where(p => p.UserId == _userManager.GetUserId(User))
                .Include(p => p.User).FirstOrDefaultAsync(m => m.Id == id);

            if (PersonalDocument == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            //ensure that only updates on current auth user
            PersonalDocument.UserId = _userManager.GetUserId(User);
            PersonalDocument old = await _context.PersonalDocument
                .AsNoTracking()
                .Where(p => p.UserId == _userManager.GetUserId(User))
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == PersonalDocument.Id);
            if (!String.Equals(old.DocumentNumber, PersonalDocument.DocumentNumber) ||
                !String.Equals(old.DocumentType, PersonalDocument.DocumentType) ||
                !String.Equals(old.FilePath, PersonalDocument.FilePath)
                )
                {
                    PersonalDocument.Valid = null;
                }
            this.ModelState.Clear();
            if (!TryValidateModel(PersonalDocument))
            {
                return Page();
            }
            
            if (Upload!=null && Upload.ContentType.Equals("application/pdf") && Upload.Length <= 1000000)
            {
                string fileName = $"{PersonalDocument.UserId}_PersonalDocument_{DateTime.Now.ToString("yyyyMMddHHmmss")}.pdf";
                var file = Path.Combine(_environment.ContentRootPath, "uploads", fileName);
                using (var fileStream = new FileStream(file, FileMode.Create))
                {
                    await Upload.CopyToAsync(fileStream);
                    if(!String.IsNullOrEmpty(old.FilePath) &&
                        System.IO.File.Exists(Path.Combine(_environment.ContentRootPath, "uploads", old.FilePath)))
                    {
                        System.IO.File.Delete(Path.Combine(_environment.ContentRootPath, "uploads", old.FilePath));
                    }
                    PersonalDocument.FilePath=fileName;
                }
            }

            _context.Attach(PersonalDocument).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonalDocumentExists(PersonalDocument.Id))
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

        private bool PersonalDocumentExists(int id)
        {
            return _context.PersonalDocument.Any(e => e.Id == id);
        }
    }
}
