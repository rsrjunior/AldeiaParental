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
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace AldeiaParental.Pages_CheckDocuments
{
    public class EditModel : PageModel
    {
        private readonly AldeiaParental.Data.ApplicationDbContext _context;
        private IWebHostEnvironment _environment;

        public EditModel(AldeiaParental.Data.ApplicationDbContext context,
        IWebHostEnvironment environment)
        {
            _context = context;
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
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);

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
            if (!ModelState.IsValid)
            {
                return Page();
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
        public ActionResult OnGetDownloadFile(string fileName)
        {
            //Build the File Path.
            string path = Path.Combine(_environment.ContentRootPath, "uploads", fileName);

            try
            {
                //Read the File data into Byte Array.
                byte[] bytes = System.IO.File.ReadAllBytes(path);
                return File(bytes, "application/octet-stream", fileName);
            }
            catch (System.Exception)
            {
               return new RedirectToPageResult("Index");
            }
            
        }
    }
}
