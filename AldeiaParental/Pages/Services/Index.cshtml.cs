using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AldeiaParental.Data;
using AldeiaParental.Models;

namespace AldeiaParental.Pages_Services
{
    public class IndexModel : PageModel
    {
        private readonly AldeiaParental.Data.ApplicationDbContext _context;

        public IndexModel(AldeiaParental.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Service> Service { get;set; }

        public async Task OnGetAsync()
        {
            Service = await _context.Service
                .Include(s => s.Caregiver)
                .Include(s => s.Customer).ToListAsync();
        }
    }
}
