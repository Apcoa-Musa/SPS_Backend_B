using GarageQueueDownload.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Text;



namespace GarageQueueDownload.Controllers
{
    [ApiController]
    [Route("api/queue")]
    public class QueueController : ControllerBase
    {
        private readonly CarParksApiService _carParksApiService;

        public QueueController(CarParksApiService carParksApiService)
        {
            _carParksApiService = carParksApiService;
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            if (!Guid.TryParse(id, out Guid parsedId))
            {
                return BadRequest(new { message = "Id måste vara ett giltigt Guid." });
            }

            try
            {
                var queue = await _carParksApiService.GetQueueById(parsedId);
                return queue != null ? Ok(queue) : NotFound(new { message = $"Ingen kö hittades med ID {id}." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ett fel uppstod vid hämtning av ködata.", error = ex.Message });
            }
        }

        [HttpGet("DownloadQueueList/{carParkId}")]
        public async Task<IActionResult> DownloadQueueList(string carParkId)
        {
            if (!Guid.TryParse(carParkId, out Guid parsedCarParkId))
            {
                return BadRequest(new { message = "CarParkId måste vara ett giltigt Guid." });
            }

            try
            {
                var queues = await _carParksApiService.GetQueueByCarParkId(parsedCarParkId);

                if (queues == null || !queues.Any())
                {
                    return NotFound(new { message = $"Inga köer hittades för CarParkId {carParkId}." });
                }

                
                var csvBuilder = new System.Text.StringBuilder();
                csvBuilder.AppendLine("Id,Name,CarParkId,ProductTemplateId,DateCreated,QueuePrice,CurrentPosition");

                int currentPosition = 1;

                Console.WriteLine("Hämtade köer från API:");
                foreach (var queue in queues)
                {
                    
                    csvBuilder.AppendLine(
                        $"{queue.QueueId},{queue.Name ?? "N/A"},{queue.CarParkId},{queue.ProductTemplateId},{queue.DateCreated:yyyy-MM-dd HH:mm:ss},{queue.QueuePrice},{currentPosition++}");

                    Console.WriteLine($"QueueId: {queue.QueueId}, CarParkId: {queue.CarParkId}");
                }

                
                var fileBytes = System.Text.Encoding.UTF8.GetBytes(csvBuilder.ToString());
                return File(fileBytes, "text/csv", $"QueueList_{carParkId}.csv");
            }
            catch (Exception ex)
            {
             
                Console.WriteLine($"Fel vid generering av CSV: {ex.Message}");
                return StatusCode(500, new { message = "Ett fel uppstod vid generering av kölistan.", error = ex.Message });
            }
        }



        [HttpGet("GetByCarParkId/{carParkId}")]
        public async Task<IActionResult> GetByCarParkId(string carParkId)
        {
            if (!Guid.TryParse(carParkId, out Guid parsedCarParkId))
            {
                return BadRequest(new { message = "CarParkId måste vara ett giltigt Guid." });
            }

            try
            {
                var queue = await _carParksApiService.GetQueueByCarParkId(parsedCarParkId);
                return queue != null ? Ok(queue) : NotFound(new { message = $"Ingen kö hittades för CarParkId {carParkId}." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ett fel uppstod vid hämtning av ködata.", error = ex.Message });
            }
        }



        [HttpGet("GetQueuesByDs/{dsNumber}")]
        [Produces("text/csv")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetQueuesByDs(string dsNumber)
        {
            if (!int.TryParse(dsNumber, out int parsedDsNumber))
            {
                Console.WriteLine($"Ogiltigt DS-nummer: {dsNumber}");
                return BadRequest(new { message = "DS-nummer måste vara ett giltigt heltal." });
            }
            try
            {
                // Hämta alla köer
                var queues = await _carParksApiService.GetAllQueues();
                Console.WriteLine($"Hämtade rådata: {System.Text.Json.JsonSerializer.Serialize(queues)}");
                // Hämta CarPark baserat på DS-nummer
                var carpark = await _carParksApiService.GetCarParkByDS(parsedDsNumber.ToString());
                if (carpark == null)
                {
                    Console.WriteLine($"Ingen CarPark hittades för DS-nummer {dsNumber}.");
                    return NotFound(new { message = $"Ingen CarPark hittades för DS-nummer {dsNumber}." });
                }
                Console.WriteLine($"Hämtade CarPark med ID {carpark.Id} för DS-nummer {dsNumber}.");
                // Filtrera köer baserat på CarParkId
                var filteredQueues = queues.Where(q => q.CarParkId == carpark.Id).ToList();
                Console.WriteLine($"Antal filtrerade köer: {filteredQueues.Count}");
                if (!filteredQueues.Any())
                {
                    Console.WriteLine($"Inga köer hittades för DS-nummer {dsNumber}. En tom CSV-fil returneras.");
                    var csvBuilder = new System.Text.StringBuilder();
                    csvBuilder.AppendLine("Meddelande");
                    csvBuilder.AppendLine($"Inga köer hittades för DS-nummer {dsNumber}.");
                    return File(Encoding.UTF8.GetBytes(csvBuilder.ToString()), "text/csv", $"Empty_QueuesByDS_{dsNumber}.csv");
                }
                // Generera CSV
                var csvBuilderWithData = new System.Text.StringBuilder();
                csvBuilderWithData.AppendLine("Id,Name,CarParkDSNumber,Priority,Description,Status,DateCreated,QueuePrice,CurrentPosition");
                int currentPosition = 1;
                foreach (var queue in filteredQueues)
                {
                    csvBuilderWithData.AppendLine(
                        $"{queue.Id},{queue.Name},{queue.CarParkDSNumber},{queue.Priority},{queue.Description},{queue.Status},{queue.DateCreated},{queue.QueuePrice},{currentPosition++}");
                }
                Console.WriteLine($"CSV-fil genererad för DS-nummer {dsNumber}.");
                var fileBytes = Encoding.UTF8.GetBytes(csvBuilderWithData.ToString());
                var fileName = $"QueuesByDS_{dsNumber}.csv";
                return File(fileBytes, "text/csv", fileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fel vid generering av CSV: {ex.Message}");
                return StatusCode(500, new { message = "Ett fel uppstod vid generering av CSV.", error = ex.Message });
            }
        }





    }
}