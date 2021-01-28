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
        public async Task<IActionResult> OnGetCsv()
        {
            var builder = new StringBuilder();
            builder.AppendLine("CuidadorId, ClienteId, Avaliação,Data,ComentarioCuidador,ComentarioCliente");
            List<Service> _services = await _context.Service.ToListAsync<Service>();
            foreach (var service in _services)
            {
                builder.Append($"{service.CaregiverId},{service.CustomerId},{(service.Rate??0)},{service.datetime}");
                builder.Append($",{(string.IsNullOrEmpty(service.CaregiverComments)?string.Empty:service.CaregiverComments.Replace(",",""))}");
                builder.AppendLine($",{(string.IsNullOrEmpty(service.CustomerComments)?string.Empty:service.CustomerComments.Replace(",",""))}");           
            }

            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "services.csv");
        }
    }
}
