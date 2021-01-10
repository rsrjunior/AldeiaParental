namespace AldeiaParental.Models
{
    public class PersonalDocument
    {
        public int Id { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNumber { get; set; }
        public string FilePath { get; set; }
        public bool? Valid { get; set; }
    }
}