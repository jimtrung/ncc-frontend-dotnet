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

        public async Task InsertTicket(Ticket ticket)
        {
            var content = JsonContent.Create(ticket, options: JsonOptions);

            var request = new HttpRequestMessage(HttpMethod.Post, "ticket")
            {
                Content = content
            };


            var token = _tokenUtil.LoadAccessToken();
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Không thể thêm vé. Trạng thái: {response.StatusCode}, Body: {body}");
            }
        }

        public List<Ticket> GetTicketsByUserId(Guid userId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"ticket/user/{userId}");


            var token = _tokenUtil.LoadAccessToken();
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            // Gọi API đồng bộ
            var response = _http.Send(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Không thể lấy vé của người dùng {userId}. Trạng thái: {response.StatusCode}");

            var body = response.Content.ReadAsStringAsync().Result;

            if (string.IsNullOrWhiteSpace(body) || body.TrimStart().StartsWith("{"))
                return new List<Ticket>();

            return JsonSerializer.Deserialize<List<Ticket>>(body, JsonOptions) ?? new List<Ticket>();
        }

        public List<Ticket> GetAllTickets()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "ticket");


            var token = _tokenUtil.LoadAccessToken();
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            // Gọi API đồng bộ
            var response = _http.Send(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Không thể lấy tất cả vé. Trạng thái: {response.StatusCode}");

            var body = response.Content.ReadAsStringAsync().Result;

            if (string.IsNullOrWhiteSpace(body) || body.TrimStart().StartsWith("{"))
                return new List<Ticket>();

            return JsonSerializer.Deserialize<List<Ticket>>(body, JsonOptions) ?? new List<Ticket>();
        }

        public void DeleteTicketById(Guid ticketId)
        {
            var result = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa vé này không?",
                "Xác nhận xóa",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            if (result != MessageBoxResult.Yes)
                return;

            // Gọi API xóa vé
            var request = new HttpRequestMessage(HttpMethod.Delete, $"ticket/{ticketId}");


            var token = _tokenUtil.LoadAccessToken();
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = _http.Send(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Không thể xóa vé {ticketId}. Trạng thái: {response.StatusCode}");
        }


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
                throw new Exception($"Không thể xóa tất cả vé. Trạng thái: {response.StatusCode}");
        }

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
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return null;

                throw new Exception($"Không thể lấy thông tin vé {ticketId}. Trạng thái: {response.StatusCode}");
            }

            var body = response.Content.ReadAsStringAsync().Result;

            if (string.IsNullOrWhiteSpace(body) || body.TrimStart().StartsWith("["))
                return null;

            return JsonSerializer.Deserialize<Ticket>(body, JsonOptions);
        }
    }
}
