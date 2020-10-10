using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AldeiaParental.Data;
using AldeiaParental.Models;

namespace AldeiaParental.Pages.ServiceLocations
{
    public class IndexModel : PageModel
    {
        private readonly AldeiaParental.Data.ApplicationDbContext _context;

        public IndexModel(AldeiaParental.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<ServiceLocation> ServiceLocation { get;set; }

        public async Task OnGetAsync()
        {
            ServiceLocation = await _context.ServiceLocation
                .Include(s => s.Region)
                .Include(s => s.User).ToListAsync();
        }
    }
}
