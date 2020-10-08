using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AldeiaParental.Models
{
    public class AldeiaParentalUser:IdentityUser
    {
        public AldeiaParentalUser() : base() { }
        public AldeiaParentalUser(string userName) : base(userName) { }
        [PersonalData]
        public string FirstName { get; set; }
        [PersonalData]
        public string LastName { get; set; }
        [PersonalData]
        public string Address { get; set; }
    }
}
