namespace Theater_Management_FE.Models
{
    public class Director
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public DateTime? Dob { get; set; }
        public int? Age { get; set; }
        public Gender Gender { get; set; }
        public string CountryCode { get; set; }
        public Country Country { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
