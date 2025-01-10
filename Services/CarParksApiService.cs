using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace GarageQueueUpload.Services
{
    public class CarParksApiService
    {
        private readonly HttpClient _httpClient;

        public CarParksApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<object> GetQueueById(int id)
        {
            var response = await _httpClient.GetAsync($"/api/queue/GetById/{id}");
            response.EnsureSuccessStatusCode();
            return JsonSerializer.Deserialize<object>(await response.Content.ReadAsStringAsync());
        }

        public async Task<object> GetAllQueues()
        {
            var response = await _httpClient.GetAsync("/api/queue/GetAll");
            response.EnsureSuccessStatusCode();
            return JsonSerializer.Deserialize<object>(await response.Content.ReadAsStringAsync());
        }

        public async Task<object> GetQueueByCarParkId(int carParkId)
        {
            var response = await _httpClient.GetAsync($"/api/queue/GetByCarParkId/{carParkId}");
            response.EnsureSuccessStatusCode();
            return JsonSerializer.Deserialize<object>(await response.Content.ReadAsStringAsync());
        }

        public async Task<object> GetQueueDetailsForGarage(int carParkId)
        {
            var response = await _httpClient.GetAsync($"/api/queue/GetQueueDetailsForGarage/{carParkId}");
            response.EnsureSuccessStatusCode();
            return JsonSerializer.Deserialize<object>(await response.Content.ReadAsStringAsync());
        }
    }
}
