using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace GarageQueueUpload.Services
{
    public class CarParksApiService
    {
        private static readonly HttpClient _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://carparks-api.sps-stage.europark.local/")
        };

        public CarParksApiService(HttpClient httpClient)
        {
            //_httpClient = httpClient;
        }

        // Använd Guid istället för string
        public async Task<QueueModel> GetQueueById(Guid id)
        {
            var response = await _httpClient.GetAsync($"/api/queue/GetById/{id}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<QueueModel>(content);
        }

        public async Task<object> GetAllQueues()
        {
            var response = await _httpClient.GetAsync("/api/queue/GetAll");
            response.EnsureSuccessStatusCode();
            return JsonSerializer.Deserialize<object>(await response.Content.ReadAsStringAsync());
        }

        public async Task<object> GetQueueByCarParkId(Guid carParkId)
        {
            var response = await _httpClient.GetAsync($"/api/queue/GetByCarParkId/{carParkId}");
            response.EnsureSuccessStatusCode();
            return JsonSerializer.Deserialize<object>(await response.Content.ReadAsStringAsync());
        }

        public async Task<object> GetQueueDetailsForGarage(Guid carParkId)
        {
            var response = await _httpClient.GetAsync($"/api/queue/GetQueueDetailsForGarage/{carParkId}");
            response.EnsureSuccessStatusCode();
            return JsonSerializer.Deserialize<object>(await response.Content.ReadAsStringAsync());
        }
    }
}
