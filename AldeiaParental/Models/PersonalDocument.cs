using System.ComponentModel.DataAnnotations;

namespace AldeiaParental.Models
{
    public class PersonalDocument
    {
        public int Id { get; set; }
        [Required]
        public string DocumentType { get; set; }
        [Required]
        public string DocumentNumber { get; set; }
        public string FilePath { get; set; }
        public bool? Valid { get; set; }
        public AldeiaParentalUser User { get; set; }
        [Required]
        public string UserId { get; set; }
    }
}