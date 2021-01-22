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
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace AldeiaParental.Areas_Identity_Pages_Account_Manage_PersonalDocuments
{
    public class DetailsModel : PageModel
    {
        private readonly AldeiaParental.Data.ApplicationDbContext _context;
        private readonly UserManager<AldeiaParentalUser> _userManager;
        private IWebHostEnvironment _environment;

        public DetailsModel(AldeiaParental.Data.ApplicationDbContext context,
        UserManager<AldeiaParentalUser> userManager,
        IWebHostEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _environment = environment;
        }

        public PersonalDocument PersonalDocument { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PersonalDocument = await _context.PersonalDocument
                .Where(p=>p.UserId==_userManager.GetUserId(User))
                .Include(p => p.User).FirstOrDefaultAsync(m => m.Id == id);

            if (PersonalDocument == null)
            {
                return NotFound();
            }
            return Page();
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
