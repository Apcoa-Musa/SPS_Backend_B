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
        private static readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("https://carparks-api.sps-stage.europark.local/") };

        public CarParksApiService(HttpClient httpClient)
        {
            //_httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }
        // Metod f�r att h�mta alla k�er
        public async Task<IEnumerable<QueueModel>> GetAllQueues()
        {
            var response = await _httpClient.GetAsync($"/api/queue/GetAll");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<QueueModel>>(content);
        }

        public async Task<CarParkDto> GetCarParkByDS(string DsNumber)
        {
            var response = await _httpClient.GetAsync($"/api/carpark/GetByDsNumber/{DsNumber}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<CarParkDto>(content);

        }
        // Metod f�r att h�mta k�er baserat p� CarParkId
        public async Task<IEnumerable<QueueModel>> GetQueueByCarParkId(Guid carParkId)
        {
            var response = await _httpClient.GetAsync($"/api/queue/GetByCarParkId/{carParkId}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<QueueModel>>(content);
        }

        // Metod f�r att h�mta en specifik k� baserat p� QueueId
        public async Task<QueueModel> GetQueueById(Guid queueId)
        {
            var response = await _httpClient.GetAsync($"/api/queue/GetById/{queueId}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<QueueModel>(content);
        }

        // Metod f�r att h�mta detaljerade k�data f�r ett garage
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
            Console.WriteLine($"H�mtade CSV-data f�r CarParkId: {carParkId}, storlek: {fileBytes.Length} byte");
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
