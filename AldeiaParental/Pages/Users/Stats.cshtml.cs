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
    public string ChartJson { get; set; }


    public StatsModel(AldeiaParental.Data.ApplicationDbContext context,
    UserManager<AldeiaParentalUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task OnGetAsync()
    {
        int cliente = 0, cuidador = 0, ambos = 0;

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
                    label: '# of Votes',");
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
        ChartJson = builder.ToString();

        // Chart = JsonConvert.DeserializeObject<ChartJs>(chartData);
        // ChartJson = JsonConvert.SerializeObject(Chart, new JsonSerializerSettings
        // {
        //     NullValueHandling = NullValueHandling.Ignore,
        // });
    }
}