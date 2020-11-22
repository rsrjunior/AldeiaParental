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

        public AldeiaParentalUser User { get; set; }
        [Required]
        public string UserId { get; set; }
        public Region Region { get; set; }
        [Required]
        public int RegionId { get; set; }

    }
}
