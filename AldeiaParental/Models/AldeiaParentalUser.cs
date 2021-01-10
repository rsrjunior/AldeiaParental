using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AldeiaParental.Models
{
    public class AldeiaParentalUser:IdentityUser
    {
        public AldeiaParentalUser() : base() { }
        public AldeiaParentalUser(string userName) : base(userName) { }
        [PersonalData]
        [Required]
        public string FirstName { get; set; }
        [PersonalData]
        [Required]
        public string LastName { get; set; }
        [Required]
        public override string PasswordHash { get; set; }
        [ProtectedPersonalData]
        [Required]
        public override string Email { get; set; }
        [PersonalData]
        public string Address { get; set; }

        public List<ServiceLocation> ServiceLocations { get; set; }

        public List<PersonalDocument> PersonalDocuments { get; set; }

    }
}
