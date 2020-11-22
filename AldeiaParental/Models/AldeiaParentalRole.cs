using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AldeiaParental.Models
{
    public class AldeiaParentalRole:IdentityRole
    {
        public AldeiaParentalRole() : base(){
            FreeWill = false;
        }
        public AldeiaParentalRole(string roleName) : base(roleName) { }
        public AldeiaParentalRole(string roleName, string description) : base(roleName)
        {
            Description = description;
            FreeWill = false;
        }
        public AldeiaParentalRole(string roleName, string description, bool freeWill) : base(roleName)
        {
            Description = description;
            FreeWill = freeWill;
        }
        public string Description { get; set; }
        public bool FreeWill { get; set; }
    }
}
