using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Linq;
using System;
using System.Collections.Generic;
using AldeiaParental.Models;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;


namespace AldeiaParental.Pages_ListServiceLocations
{
    public class StatsModel : PageModel
    {
        private readonly AldeiaParental.Data.ApplicationDbContext _context;
        public string CaregiverRegionChartJson { get; set; }

        public StatsModel(AldeiaParental.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        public void OnGet()
        {
            List<string> regions = new List<string>();
            List<int> countRegions = new List<int>();
            
            var result = _context.ServiceLocation
                .Include(s => s.Region)
                .Include(s => s.User)
                .Select(s => new {s.UserId, s.Region.Name})
                .Distinct()
                .ToList()
                .GroupBy(r => r.Name);
            
            foreach (var r in result)
            {
                regions.Add(r.Key);
                countRegions.Add(r.Count());
            }

            var builder = new StringBuilder();
            builder.Append("{type: 'horizontalBar',responsive: true,data:{");
            builder.Append($"labels: {JsonSerializer.Serialize(regions)},");
            builder.Append("datasets: [");              
            builder.Append("{label: 'num. cuidadores',");
            builder.Append($"data: {JsonSerializer.Serialize(countRegions)}");
            builder.Append(@"},");
            builder.Append("]},options: {title: {display: true,text: 'Cuidadores por Região'}, scales: { xAxes: [{ ticks: { beginAtZero: true } }] } }}");

            CaregiverRegionChartJson = builder.ToString();

        }
    }
}