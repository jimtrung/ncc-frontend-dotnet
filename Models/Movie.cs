using System.ComponentModel;

namespace Theater_Management_FE.Models
{
    public class Movie : INotifyPropertyChanged
    {
        private Guid _id;
        private string _name;
        private string _description;
        private Guid? _directorId;
        private List<MovieGenre> _genres;
        private DateTime? _premiere;
        private int? _duration;
        private string _language;
        private int? _rated;
        private DateTime _createdAt;
        private DateTime _updatedAt;
        public Guid Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(nameof(Id)); }
        }
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }
        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(nameof(Description)); }
        }
        public Guid? DirectorId
        {
            get => _directorId;
            set { _directorId = value; OnPropertyChanged(nameof(DirectorId)); }
        }

        // DirectorName for UI display only - not sent to backend
        [System.Text.Json.Serialization.JsonIgnore]
        private string _directorName;
        [System.Text.Json.Serialization.JsonIgnore]
        public string DirectorName
        {
            get => _directorName ?? "";
            set { _directorName = value; OnPropertyChanged(nameof(DirectorName)); }
        }

        public List<MovieGenre> Genres
        {
            get => _genres;
            set { _genres = value; OnPropertyChanged(nameof(Genres)); OnPropertyChanged(nameof(VietnameseGenres)); }
        }

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
                "sci-fi" => "Khoa học viễn tưởng",
                "thriller" => "Giật gân",
                "animation" => "Hoạt hình",
                "fantasy" => "Giả tưởng",
                "documentary" => "Tài liệu",
                "family" => "Gia đình",
                "crime" => "Tội phạm",
                "mystery" => "Bí ẩn",
                _ => genre // Return original if no translation found
            };
        }
        public DateTime? Premiere
        {
            get => _premiere;
            set { _premiere = value; OnPropertyChanged(nameof(Premiere)); }
        }
        public int? Duration
        {
            get => _duration;
            set { _duration = value; OnPropertyChanged(nameof(Duration)); }
        }
        public string Language
        {
            get => _language;
            set { _language = value; OnPropertyChanged(nameof(Language)); }
        }
        public int? Rated
        {
            get => _rated;
            set { _rated = value; OnPropertyChanged(nameof(Rated)); }
        }
        public DateTime CreatedAt
        {
            get => _createdAt;
            set { _createdAt = value; OnPropertyChanged(nameof(CreatedAt)); }
        }
        public DateTime UpdatedAt
        {
            get => _updatedAt;
            set { _updatedAt = value; OnPropertyChanged(nameof(UpdatedAt)); }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
