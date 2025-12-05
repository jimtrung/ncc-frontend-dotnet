
namespace Theater_Management_FE.Models
{
    public class Ticket
    {
        public Guid Id { get; set; }
        public Guid Userid { get; set; }
        public Guid Showtimeid { get; set; }
        public string Seatname { get; set; }
        public int Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
