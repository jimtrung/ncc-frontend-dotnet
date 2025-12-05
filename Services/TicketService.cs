// TicketService.cs
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Theater_Management_FE.Models;
using Theater_Management_FE.Utils;
using System.Threading.Tasks;
using System.Windows;

namespace Theater_Management_FE.Services
{
    public class TicketService
    {
        private readonly HttpClient _http;
        private readonly AuthTokenUtil _tokenUtil;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DictionaryKeyPolicy = null,
            DefaultIgnoreCondition = JsonIgnoreCondition.Never,
            WriteIndented = false
        };

        public TicketService(HttpClient http, AuthTokenUtil tokenUtil)
        {
            _http = http;
            _http.BaseAddress = new Uri("http://localhost:8080/");
            _tokenUtil = tokenUtil;
        }

        // INSERT NEW TICKET
        public async Task InsertTicket(Ticket ticket)
        {
            var content = JsonContent.Create(ticket, options: JsonOptions);

            var request = new HttpRequestMessage(HttpMethod.Post, "ticket")
            {
                Content = content
            };

            // Thêm header Authorization nếu có token
            var token = _tokenUtil.LoadAccessToken();
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _http.SendAsync(request);

            // Nếu server trả lỗi, ném exception
            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Failed to insert ticket. Status: {response.StatusCode}, Body: {body}");
            }
        }

        // GET TICKETS BY USER ID
        public List<Ticket> GetTicketsByUserId(Guid userId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"ticket/user/{userId}");

            // Thêm header Authorization nếu có token
            var token = _tokenUtil.LoadAccessToken();
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            // Gọi API đồng bộ
            var response = _http.Send(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Failed to get tickets for user {userId}. Status: {response.StatusCode}");

            var body = response.Content.ReadAsStringAsync().Result;

            // Nếu body rỗng hoặc trả về object thay vì array, trả về danh sách rỗng
            if (string.IsNullOrWhiteSpace(body) || body.TrimStart().StartsWith("{"))
                return new List<Ticket>();

            return JsonSerializer.Deserialize<List<Ticket>>(body, JsonOptions) ?? new List<Ticket>();
        }

        public List<Ticket> GetAllTickets()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "ticket");

            // Thêm header Authorization nếu có token
            var token = _tokenUtil.LoadAccessToken();
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            // Gọi API đồng bộ
            var response = _http.Send(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Failed to get all tickets. Status: {response.StatusCode}");

            var body = response.Content.ReadAsStringAsync().Result;

            // Nếu body rỗng hoặc trả về object thay vì array, trả về danh sách rỗng
            if (string.IsNullOrWhiteSpace(body) || body.TrimStart().StartsWith("{"))
                return new List<Ticket>();

            return JsonSerializer.Deserialize<List<Ticket>>(body, JsonOptions) ?? new List<Ticket>();
        }

        // Xóa vé theo ID
        public void DeleteTicketById(Guid ticketId)
        {
            // Hiển thị hộp thoại xác nhận
            var result = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa vé này không?",
                "Xác nhận xóa",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            if (result != MessageBoxResult.Yes)
                return; // Người dùng hủy, không xóa

            // Gọi API xóa vé
            var request = new HttpRequestMessage(HttpMethod.Delete, $"ticket/{ticketId}");

            var token = _tokenUtil.LoadAccessToken();
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = _http.Send(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Failed to delete ticket {ticketId}. Status: {response.StatusCode}");
        }


        // Xóa tất cả vé
        public void DeleteAllTickets()
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, "ticket");

            var token = _tokenUtil.LoadAccessToken();
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = _http.Send(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Failed to delete all tickets. Status: {response.StatusCode}");
        }

        // GET TICKET BY ID
        public Ticket? GetTicketById(Guid ticketId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"ticket/{ticketId}");

            // Thêm header Authorization nếu có token
            var token = _tokenUtil.LoadAccessToken();
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            // Gọi API đồng bộ
            var response = _http.Send(request);

            if (!response.IsSuccessStatusCode)
            {
                // Nếu không tìm thấy, trả về null
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return null;

                throw new Exception($"Failed to get ticket {ticketId}. Status: {response.StatusCode}");
            }

            var body = response.Content.ReadAsStringAsync().Result;

            if (string.IsNullOrWhiteSpace(body) || body.TrimStart().StartsWith("["))
                return null; // Body rỗng hoặc trả về array không hợp lệ

            return JsonSerializer.Deserialize<Ticket>(body, JsonOptions);
        }
    }
}
