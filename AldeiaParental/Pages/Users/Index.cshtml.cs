using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AldeiaParental.Models;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace AldeiaParental.Pages_Users
{
    public class IndexModel : PageModel
    {
        private readonly AldeiaParental.Data.ApplicationDbContext _context;
        private readonly UserManager<AldeiaParentalUser> _userManager;

        public IndexModel(AldeiaParental.Data.ApplicationDbContext context,
        UserManager<AldeiaParentalUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<UserViewModel> Users { get;set; }

        public async Task OnGetAsync()
        {
            List<AldeiaParentalUser> _users = await _context.Users.ToListAsync();
            Users = new List<UserViewModel>();
            foreach (var user in _users)
            {
                Users.Add(new UserViewModel
                        {
                            Id = user.Id,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email,
                            LockoutEnd = user.LockoutEnd,
                            Perfis = await _userManager.GetRolesAsync(user),
                            RegistrationDate = user.RegistrationDate                           
                        });
            }
        }
        
        public class UserViewModel
        {
            //Nome	Sobrenome	Email	Perfil	RegistrationDate	LockoutEnd
            public string Id { get; set; }
            [Display(Name = "Nome")]
            public string FirstName { get; set; }
            [Display(Name = "Sobrenome")]
            public string LastName { get; set; }
            public string Email { get; set; }
            public IList<string> Perfis { get; set; }
            [Display(Name = "Cadastro")]
            public DateTime? RegistrationDate { get; set; }
            [Display(Name = "Fim Bloqueio")]
            public DateTimeOffset? LockoutEnd { get; set; }
        }
        public async Task<IActionResult> OnGetCsv()
        {
            var builder = new StringBuilder();
            builder.AppendLine("Id,Nome,Sobrenome,Email,Perfis,Cadastro,FimBloqueio");
            List<AldeiaParentalUser> _users = _context.Users.ToList<AldeiaParentalUser>();
            foreach (var user in _users)
            {
                builder.Append($"{user.Id},{user.FirstName},{user.LastName},{user.Email},");
                IList<string> roles = await _userManager.GetRolesAsync(user);
                foreach (string item in roles)
                {
                    builder.Append($"{item}");
                }
                builder.AppendLine($",{user.RegistrationDate},{user.LockoutEnd}");
            }

            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "users.csv");
        }
    }
}
