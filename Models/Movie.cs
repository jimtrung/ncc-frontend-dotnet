using System.Text.Json.Serialization;

namespace Theater_Management_FE.Models
{
    public class Movie
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? DirectorId { get; set; }
        [JsonIgnore]
        public Director Director { get; set; }

        [JsonIgnore]
        public string DirectorName => Director?.FullName ?? "";
        public List<MovieGenre> Genres { get; set; }

        [JsonIgnore]
        public string VietnameseGenres
        {
            get
            {
                if (Genres == null || !Genres.Any()) return "Chưa cập nhật";
                var vietnameseGenres = Genres.Select(g => TranslateGenre(g.ToString())).ToList();
                return string.Join(", ", vietnameseGenres);
            }
        }

        private string TranslateGenre(string genre)
        {
            return genre?.ToLower() switch
            {
                "action" => "Hành động",
                "adventure" => "Phiêu lưu",
                "comedy" => "Hài",
                "drama" => "Chính kịch",
                "horror" => "Kinh dị",
                "romance" => "Lãng mạn",
                "science_fiction" => "Khoa học viễn tưởng",
                "thriller" => "Giật gân",
                "animation" => "Hoạt hình",
                "fantasy" => "Giả tưởng",
                "documentary" => "Tài liệu",
                "family" => "Gia đình",
                "crime" => "Tội phạm",
                "mystery" => "Bí ẩn",
                "musical" => "Nhạc kịch",
                "war" => "Chiến tranh",
                "western" => "Miền Tây",
                "historical" => "Lịch sử",
                "sports" => "Thể thao",
                "biography" => "Tiểu sử",
                _ => genre 
            };
        }
        public DateTime? Premiere { get; set; }
        public int? Duration { get; set; }
        public string Language { get; set; }
        public int? Rated { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
