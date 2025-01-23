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

        // Generisk metod för att deserialisera API-svar
        private async Task<T> GetDeserializedResponse<T>(string endpoint)
        {
            try
            {
                var response = await _httpClient.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? throw new Exception($"Tom respons från {endpoint}");
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine($"HTTP Request Error: {httpEx.Message}");
                throw;
            }
            catch (JsonException jsonEx)
            {
                Console.WriteLine($"JSON Parsing Error: {jsonEx.Message}");
                throw;
            }
        }

        
        public async Task<IEnumerable<QueueModel>> GetAllQueues()
        {
            return await GetDeserializedResponse<IEnumerable<QueueModel>>("/api/queue/GetAll");
        }

        
        public async Task<QueueModel> GetCarParkByDS(string DsNumber)
        {
            return await GetDeserializedResponse<QueueModel>($"/api/carpark/GetByDsNumber/{DsNumber}");
        }

        
        public async Task<IEnumerable<QueueModel>> GetQueueByCarParkId(Guid carParkId)
        {
            return await GetDeserializedResponse<IEnumerable<QueueModel>>($"/api/queue/GetByCarParkId/{carParkId}");
        }

        
        public async Task<QueueModel> GetQueueById(Guid Id)
        {
            return await GetDeserializedResponse<QueueModel>($"/api/queue/GetById/{Id}");
        }

     

        
        public async Task<byte[]> DownloadQueueList(Guid carParkId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/queue/DownloadQueueList/{carParkId}");
                response.EnsureSuccessStatusCode();

                var fileBytes = await response.Content.ReadAsByteArrayAsync();
                Console.WriteLine($"Hämtade CSV-data för CarParkId: {carParkId}, storlek: {fileBytes.Length} byte");
                return fileBytes;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fel vid hämtning av CSV: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<QueueModel>> GetQueuesByDs(int dsNumber)
        {
            return await GetDeserializedResponse<IEnumerable<QueueModel>>($"/api/queue/GetQueuesByDs/{dsNumber}");
        }
    }
}
