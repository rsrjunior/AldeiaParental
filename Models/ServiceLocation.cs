using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AldeiaParental.Models
{
    public class ServiceLocation
    {
        public int Id { get; set; }
        public bool AtCustomerHome { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }

        [Required]
        public AldeiaParentalUser User { get; set; }
        public string UserId { get; set; }
        [Required]
        public Region Region { get; set; }
        public int RegionId { get; set; }

    }
}
