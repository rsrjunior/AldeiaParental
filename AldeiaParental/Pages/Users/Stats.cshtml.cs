using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Collections.Generic;
using AldeiaParental.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class StatsModel : PageModel
{
    private readonly AldeiaParental.Data.ApplicationDbContext _context;
    private readonly UserManager<AldeiaParentalUser> _userManager;
    public string UsersProfileChartJson { get; set; }
    public string MonthUsersChartJson { get; set; }


    public StatsModel(AldeiaParental.Data.ApplicationDbContext context,
    UserManager<AldeiaParentalUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    private int getMonthIndex(int increment) => System.DateTime.Now.AddMonths(increment).Month;
    
    private string getMonthLabel(int increment) => 
        System.DateTime.Now.AddMonths(increment).ToString("MMM-yy", System.Globalization.CultureInfo.CreateSpecificCulture("pt-BR"));

    public async Task OnGetAsync()
    {
        int cliente = 0, cuidador = 0, ambos = 0;
        int[] monthRegister = new int[13];

        List<AldeiaParentalUser> _users = await _context.Users.ToListAsync();
        foreach (var user in _users)
        {
            IList<string> roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("Cliente") && roles.Contains("Cuidador"))
                ambos++;
            else if (roles.Contains("Cliente"))
                cliente++;
            else if (roles.Contains("Cuidador"))
                cuidador++;

            if(user.RegistrationDate.HasValue && user.RegistrationDate > (System.DateTime.Now.AddYears(-1)))
                monthRegister[user.RegistrationDate.Value.Month]++;
        }


        var builder = new StringBuilder();
        builder.Append(@"
        {
            type: 'doughnut',
            responsive: true,
            data:
            {
                labels: ['Cliente', 'Cuidador', 'Ambos'],
                datasets: [{
                    label: '# usuários',");
        builder.Append($"data: [{cliente}, {cuidador}, {ambos}],");
        builder.Append(@"backgroundColor: [
                    'rgba(255, 99, 132, 0.5)',
                    'rgba(54, 162, 235, 0.5)',
                    'rgba(255, 206, 86, 0.5)'
                        ],
                    borderColor: [
                    'rgba(255, 99, 132, 1)',
                    'rgba(54, 162, 235, 1)',
                    'rgba(255, 206, 86, 1)'
                        ],
                    borderWidth: 1
                }]
            },
             options: {
                 title: {
                     display: true,
                     text: 'Usuários por perfil'
                     }
                }
        }");

        // Ref: https://www.chartjs.org/docs/latest/
        UsersProfileChartJson = builder.ToString();

        builder.Clear();
        builder.Append("{type: 'line',responsive: true,data:{");

        builder.Append($"labels: ['{getMonthLabel(-11)}', '{getMonthLabel(-10)}', '{getMonthLabel(-9)}', '{getMonthLabel(-8)}',  ");
        builder.Append($"'{getMonthLabel(-7)}', '{getMonthLabel(-6)}', '{getMonthLabel(-5)}', '{getMonthLabel(-4)}',  ");
        builder.Append($"'{getMonthLabel(-3)}', '{getMonthLabel(-2)}', '{getMonthLabel(-1)}', '{getMonthLabel(0)}'],");
        builder.Append("datasets: [{label: 'novos cadastros',");
        builder.Append($"data: [{monthRegister[getMonthIndex(-11)]}, {monthRegister[getMonthIndex(-10)]}, {monthRegister[getMonthIndex(-9)]},");
        builder.Append($"{monthRegister[getMonthIndex(-8)]}, {monthRegister[getMonthIndex(-7)]}, {monthRegister[getMonthIndex(-6)]},");
        builder.Append($"{monthRegister[getMonthIndex(-5)]}, {monthRegister[getMonthIndex(-4)]}, {monthRegister[getMonthIndex(-3)]},");
        builder.Append($"{monthRegister[getMonthIndex(-2)]}, {monthRegister[getMonthIndex(-1)]}, {monthRegister[getMonthIndex(0)]}],");
        builder.Append(@"backgroundColor: ['rgba(54, 162, 235, 0.5)'],
                    borderColor: ['rgba(54, 162, 235, 1)'],
                    borderWidth: 1
                    }]
            },
             options: {
                 title: {
                     display: true,
                     text: 'Cadastro mensal'
                     }
                }
        }");

        MonthUsersChartJson = builder.ToString();
        // Chart = JsonConvert.DeserializeObject<ChartJs>(chartData);
        // ChartJson = JsonConvert.SerializeObject(Chart, new JsonSerializerSettings
        // {
        //     NullValueHandling = NullValueHandling.Ignore,
        // });
    }
}