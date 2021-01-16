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

namespace AldeiaParental.Pages.Regions
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Region> Region { get;set; }

        public async Task OnGetAsync()
        {
            Region = await _context.Region.ToListAsync();
        }
        public async Task<IActionResult> OnGetCsv()
        {
            var builder = new StringBuilder();
            builder.AppendLine("Id,Nome,Descrição");
            List<Region> _regions = await _context.Region.ToListAsync<Region>();
            foreach (var region in _regions)
            {
                builder.AppendLine($"{region.Id},{region.Name},{region.Description}");
            }

            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "regions.csv");
        }
    }
}
