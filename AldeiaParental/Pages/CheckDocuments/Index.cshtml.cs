using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AldeiaParental.Data;
using AldeiaParental.Models;

namespace AldeiaParental.Pages_CheckDocuments
{
    public class IndexModel : PageModel
    {
        private readonly AldeiaParental.Data.ApplicationDbContext _context;

        public IndexModel(AldeiaParental.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<PersonalDocument> PersonalDocument { get; set; }

        public async Task OnGetAsync()
        {
            PersonalDocument = await _context.PersonalDocument
                .Include(p => p.User).ToListAsync();
        }
    }
}
