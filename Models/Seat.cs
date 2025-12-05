
namespace Theater_Management_FE.Models
{
    public class Seat
    {
        public Guid Id { get; set; }
        public Guid AuditoriumId { get; set; }
        public Auditorium Auditorium { get; set; }
        public string Row { get; set; }
        public int Number { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
