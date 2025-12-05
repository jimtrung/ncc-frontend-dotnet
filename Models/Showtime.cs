
using System;

namespace Theater_Management_FE.Models
{
    public class Showtime
    {
        public Guid Id { get; set; }
        public Guid MovieId { get; set; }
        public Guid AuditoriumId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime ShowDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string MovieName { get; set; }
        public string AuditoriumName { get; set; }
        public int Quantity { get; set; }

        // --- Readonly string properties for display ---
        public string StartTimeString => StartTime.ToUniversalTime().ToString("HH:mm");
        public string EndTimeString => EndTime.ToUniversalTime().ToString("HH:mm");
        public string ShowDateString => ShowDate.ToString("yyyy-MM-dd");
    }
}
