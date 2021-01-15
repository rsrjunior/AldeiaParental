using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AldeiaParental.Data;
using AldeiaParental.Models;
using System.Text;

namespace AldeiaParental.Pages_Users
{
    public class IndexModel : PageModel
    {
        private readonly AldeiaParental.Data.ApplicationDbContext _context;

        public IndexModel(AldeiaParental.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<AldeiaParentalUser> AldeiaParentalUser { get;set; }

        public async Task OnGetAsync()
        {
            AldeiaParentalUser = await _context.Users.ToListAsync();
        }
        
        public IActionResult OnGetCsv()
        {
            var builder = new StringBuilder();
            builder.AppendLine("Id,Username,Email");
            List<AldeiaParentalUser> _users = _context.Users.ToList<AldeiaParentalUser>();
            foreach (var user in _users)
            {
                builder.AppendLine($"{user.Id},{user.FirstName},{user.Email}");
            }

            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "users.csv");
        }
    }
}
