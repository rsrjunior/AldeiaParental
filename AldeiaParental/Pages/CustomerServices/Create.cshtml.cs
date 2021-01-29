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

namespace AldeiaParental.Pages_CustomerServices
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

        public async Task<IActionResult> OnGet(string id)
        {
            IList<AldeiaParentalUser> caregivers = await _userManager.GetUsersInRoleAsync("Cuidador");
            ViewData["Caregiver"] = new SelectList(caregivers.OrderBy(c => c.Email), "Id", "Email", id);

            return Page();
        }

        [BindProperty]
        public Service Service { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            //setar o userId para o Id do Usuario da seção
            Service.CustomerId = user.Id;
            this.ModelState.Clear();
            if (!TryValidateModel(Service))
            {
                return Page();
            }

            if (Service.datetime<(user.RegistrationDate??DateTime.Now))
            {
                 IList<AldeiaParentalUser> caregivers = await _userManager.GetUsersInRoleAsync("Cuidador");
                ViewData["Caregiver"] = new SelectList(caregivers.OrderBy(c => c.Email), "Id", "Email");
                ModelState.AddModelError(string.Empty, $"Informe a data do atendimento a partir de {(user.RegistrationDate??DateTime.Now).ToString("dd/MM/yyyy")}");
                return Page();
            }
            
            if (Service.CaregiverId==null)
            {
                IList<AldeiaParentalUser> caregivers = await _userManager.GetUsersInRoleAsync("Cuidador");
                ViewData["Caregiver"] = new SelectList(caregivers.OrderBy(c => c.Email), "Id", "Email");
                ModelState.AddModelError(string.Empty, $"Selecione o Cuidador");
                return Page();
            }


            _context.Service.Add(Service);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
