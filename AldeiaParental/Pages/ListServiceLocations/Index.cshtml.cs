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

namespace AldeiaParental.Pages_ListServiceLocations
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
        public async Task<IActionResult> OnGetCsv()
        {
            var builder = new StringBuilder();
            builder.AppendLine("CuidadorId, RegionId, Descrição");
            List<ServiceLocation> _serviceLocations = await _context.ServiceLocation.ToListAsync<ServiceLocation>();
            foreach (var location in _serviceLocations)
            {
                builder.AppendLine($"{location.UserId},{location.RegionId},{(location.AtCustomerHome?"(à domicílio)":location.Description)}");
            }

            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "servicelocations.csv");
        }
    }
}
