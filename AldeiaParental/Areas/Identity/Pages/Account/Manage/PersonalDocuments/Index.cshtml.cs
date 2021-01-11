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

namespace AldeiaParental.Areas_Identity_Pages_Account_Manage_PersonalDocuments
{
    public class IndexModel : PageModel
    {
        private readonly AldeiaParental.Data.ApplicationDbContext _context;
        private readonly UserManager<AldeiaParentalUser> _userManager;

        public IndexModel(AldeiaParental.Data.ApplicationDbContext context,
         UserManager<AldeiaParentalUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<PersonalDocument> PersonalDocument { get;set; }

        public async Task OnGetAsync()
        {
            string userId = _userManager.GetUserId(User);
            PersonalDocument = await _context.PersonalDocument
                .Where(p => p.UserId == userId)
                .Include(p => p.User).ToListAsync();
        }
    }
}
