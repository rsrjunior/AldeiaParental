using System;
using System.ComponentModel.DataAnnotations;

namespace AldeiaParental.Models
{
    public class Service
    {
        public int Id { get; set; }
        public AldeiaParentalUser Caregiver { get; set; }
        public string CaregiverId { get; set; }
        public AldeiaParentalUser Customer { get; set; }
        public string CustomerId { get; set; }
        public int? Rate { get; set; }
        public string CaregiverComments { get; set; }
        public string CustomerComments { get; set; }
        [Required]
        public DateTime datetime { get; set; }
    }
}
