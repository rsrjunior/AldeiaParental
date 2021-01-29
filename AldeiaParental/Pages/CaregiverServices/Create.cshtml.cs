using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using AldeiaParental.Data;
using AldeiaParental.Models;
using Microsoft.AspNetCore.Identity;

namespace AldeiaParental.Pages_CaregiverServices
{
    public class CreateModel : PageModel
    {
        private readonly AldeiaParental.Data.ApplicationDbContext _context;
        private readonly UserManager<AldeiaParentalUser> _userManager;

        public CreateModel(AldeiaParental.Data.ApplicationDbContext context,
            UserManager<AldeiaParentalUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGet()
        {
            IList<AldeiaParentalUser> customers = await _userManager.GetUsersInRoleAsync("Cliente");
            ViewData["Customer"] = new SelectList(customers.OrderBy(c => c.Email), "Id", "Email");
            return Page();
        }

        [BindProperty]
        public Service Service { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            //setar o userId para o Id do Usuario da seção.
            Service.CaregiverId = user.Id;
            this.ModelState.Clear();
            if (!TryValidateModel(Service))
            {
                return Page();
            }
            if (Service.datetime<(user.RegistrationDate??DateTime.Now))
            {
                IList<AldeiaParentalUser> customers = await _userManager.GetUsersInRoleAsync("Cliente");
                ViewData["Customer"] = new SelectList(customers.OrderBy(c => c.Email), "Id", "Email");
                ModelState.AddModelError(string.Empty, $"Informe a data do atendimento a partir de {(user.RegistrationDate??DateTime.Now).ToString("dd/MM/yyyy")}");
                return Page();
            }
            
            if (Service.CaregiverId==null)
            {
                IList<AldeiaParentalUser> customers = await _userManager.GetUsersInRoleAsync("Cliente");
                ViewData["Customer"] = new SelectList(customers.OrderBy(c => c.Email), "Id", "Email");
                ModelState.AddModelError(string.Empty, $"Selecione o Cliente");
                return Page();
            }

            _context.Service.Add(Service);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
