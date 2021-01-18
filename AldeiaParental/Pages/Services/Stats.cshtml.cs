using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Linq;
using System;
using AldeiaParental.Models;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;

namespace AldeiaParental.Pages_Services
{
    public class StatsModel : PageModel
    {
        private readonly AldeiaParental.Data.ApplicationDbContext _context;
        private readonly UserManager<AldeiaParentalUser> _userManager;
        public string ServicesCountChartJson { get; set; }

        public string ServicesAvgChartJson { get; set; }

        public StatsModel(AldeiaParental.Data.ApplicationDbContext context,
        UserManager<AldeiaParentalUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private int getMonthIndex(int increment) => System.DateTime.Now.AddMonths(increment).Month;

        private string getMonthLabel(int increment) => getYearMonthStr(DateTime.Now.AddMonths(increment));
        private static string getYearMonthStr(DateTime d) => d.ToString("MMM-yy", System.Globalization.CultureInfo.CreateSpecificCulture("pt-BR"));
        public void OnGet()
        {
            string[] keyServices = new string[12];
            int[] countServices = new int[12];
            double[] avgRateServices = new double[12];
            DateTime treshold = System.DateTime.Now.AddYears(-1);
            var result = 
            (from s in _context.Service
            where s.datetime > treshold
            select new {Id = s.Id, Datetime = getYearMonthStr(s.datetime), Rate = s.Rate}).ToList();

            var resultGroup = (from s in result
            group s by s.Datetime into sGroup
            select new {Key = sGroup.Key, Count = sGroup.Count(), Avg = sGroup.Average(s =>s.Rate??0)}).ToList();

            for (int i = -11; i <= 0; i++)
            {
                keyServices[i+11] = getMonthLabel(i);
                var groupedResult = resultGroup.FirstOrDefault(s=>s.Key==getMonthLabel(i));
                countServices[i+11] = groupedResult!=null?groupedResult.Count:0;
                avgRateServices[i+11] =groupedResult!=null?groupedResult.Avg:0;
            }
            
            var builder = new StringBuilder();
            builder.Append("{type: 'line',responsive: true,data:{");
            builder.Append($"labels: {JsonSerializer.Serialize(keyServices)},");
            builder.Append("datasets: [");              
            builder.Append("{label: 'num. atendimentos',");
            builder.Append($"data: {JsonSerializer.Serialize(countServices)},");
            builder.Append(@"backgroundColor: ['rgba(54, 162, 235, 0.5)'],
                    borderColor: ['rgba(54, 162, 235, 1)'],
                    borderWidth: 1
                    },");
            builder.Append("]},options: {title: {display: true,text: 'Atendimentos'}}}");

            ServicesCountChartJson = builder.ToString();


            builder.Clear();
            builder.Append("{type: 'line',responsive: true,data:{");
            builder.Append($"labels: {JsonSerializer.Serialize(keyServices)},");
            builder.Append("datasets: [");
            builder.Append("{label: 'aval. atendimentos',");
            builder.Append($"data: {JsonSerializer.Serialize(avgRateServices)},");
            builder.Append(@"backgroundColor: ['rgba(255, 206, 86, 0.5)'],
                    borderColor: ['rgba(255, 206, 86, 1)'],
                    borderWidth: 1
                    },");
            builder.Append("]},options: {title: {display: true,text: 'Atendimentos'}}}");

            ServicesAvgChartJson = builder.ToString();
        }
    }
}