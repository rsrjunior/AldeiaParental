using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AldeiaParental.Data;
using AldeiaParental.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AldeiaParental.Pages.FindCaregivers
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AldeiaParentalUser> _userManager;
        private readonly SignInManager<AldeiaParentalUser> _signInManager;
        private readonly RoleManager<AldeiaParentalRole> _roleManager;

        private const string _caregiverRole = "Cuidador";
        public IndexModel(ApplicationDbContext context,
                UserManager<AldeiaParentalUser> userManager,
                SignInManager<AldeiaParentalUser> signInManager,
                RoleManager<AldeiaParentalRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        public class Caregiver
        {
            public string UserId { get; set; }
            public string FirstName { get; set; }
        }
        public class RegionFilter
        {
            public int? RegionId { get; set; }
            public string RegionName { get; set; }
        }
        public IList<Caregiver> Caregivers { get; set; }

        public int? SelectedRegionId { get; set; }
        public SelectList RegionsList { get; set; }
        public bool? AtHome { get; set; }
        public async Task<IActionResult> OnGetAsync(int? regionId, bool? atHome)
        {
            IQueryable<ServiceLocation> serviceLocations = _context.ServiceLocation
                .Include(s => s.Region)
                .Include(s => s.User);

            SelectedRegionId = regionId;

            if (SelectedRegionId != null)
            {
                serviceLocations = serviceLocations.Where(s => s.RegionId == SelectedRegionId);
            }
            AtHome = atHome;
            if (AtHome != null)
            {
                serviceLocations = serviceLocations.Where(s => s.AtCustomerHome == AtHome);
            }
            var userList = serviceLocations.Select(s => new Caregiver()
            {
                UserId = s.UserId,
                FirstName = s.User.FirstName
            }).Distinct();

            Caregivers = await userList.ToListAsync();

            RegionsList = new SelectList(_context.Region, "Id", "Name");

            return Page();
        }
    }
}
