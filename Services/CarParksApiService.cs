using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using GarageQueueUpload.Models;

namespace GarageQueueUpload.Services
{
    public class CarParksApiService
    {
        private readonly HttpClient _httpClient;

        public CarParksApiService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        
        public async Task<IEnumerable<QueueModel>> GetAllQueues()
        {
            var response = await _httpClient.GetAsync($"/api/queue/GetAll");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"API Response: {content}");
            return JsonSerializer.Deserialize<IEnumerable<QueueModel>>(content);
        }

        public async Task<QueueModel> GetCarParkByDS(string DsNumber)
        {
            var response = await _httpClient.GetAsync($"/api/carpark/GetByDsNumber/{DsNumber}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<QueueModel>(content);

        }
        
        public async Task<IEnumerable<QueueModel>> GetQueueByCarParkId(Guid carParkId)
        {
            var response = await _httpClient.GetAsync($"/api/queue/GetByCarParkId/{carParkId}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<QueueModel>>(content);
        }

        
        public async Task<QueueModel> GetQueueById(Guid queueId)
        {
            var response = await _httpClient.GetAsync($"/api/queue/GetById/{queueId}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<QueueModel>(content);
        }

        
        public async Task<object> GetQueueDetailsForGarage(Guid carParkId)
        {
            var response = await _httpClient.GetAsync($"/api/queue/GetQueueDetailsForGarage/{carParkId}");
            response.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<object>(await response.Content.ReadAsStringAsync());
        }
       
        public async Task<byte[]> DownloadQueueList(Guid carParkId)
        {
            var response = await _httpClient.GetAsync($"/api/queue/DownloadQueueList/{carParkId}");
            response.EnsureSuccessStatusCode();

           
            var fileBytes = await response.Content.ReadAsByteArrayAsync();
            Console.WriteLine($"Hämtade CSV-data för CarParkId: {carParkId}, storlek: {fileBytes.Length} byte");
            return fileBytes;
        }


        
        public async Task<IEnumerable<QueueModel>> GetQueuesByDs(int dsNumber)
        {
            var response = await _httpClient.GetAsync($"/api/queue/GetQueuesByDs/{dsNumber}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<QueueModel>>(content);
        }
    }
}
