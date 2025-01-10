using GarageQueueUpload.Services;
using Microsoft.AspNetCore.Mvc;

namespace GarageQueueUpload.Controllers
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
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest(new { message = "Id är obligatoriskt och får inte vara tomt." });

            try
            {
                var queue = await _carParksApiService.GetQueueById(id);
                return queue != null ? Ok(queue) : NotFound(new { message = $"Ingen kö hittades med ID {id}." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ett fel uppstod vid hämtning av ködata.", error = ex.Message });
            }
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var queues = await _carParksApiService.GetAllQueues();
                return Ok(queues);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ett fel uppstod vid hämtning av alla köer.", error = ex.Message });
            }
        }

        [HttpGet("GetByCarParkId/{carParkId}")]
        public async Task<IActionResult> GetByCarParkId(string carParkId)
        {
            if (string.IsNullOrWhiteSpace(carParkId))
                return BadRequest(new { message = "carParkId är obligatoriskt och får inte vara tomt." });

            try
            {
                var queue = await _carParksApiService.GetQueueByCarParkId(carParkId);
                return queue != null ? Ok(queue) : NotFound(new { message = $"Ingen kö hittades för CarParkId {carParkId}." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ett fel uppstod vid hämtning av ködata.", error = ex.Message });
            }
        }

        [HttpGet("GetQueueDetailsForGarage/{carParkId}")]
        public async Task<IActionResult> GetQueueDetailsForGarage(string carParkId)
        {
            if (string.IsNullOrWhiteSpace(carParkId))
                return BadRequest(new { message = "carParkId är obligatoriskt och får inte vara tomt." });

            try
            {
                var details = await _carParksApiService.GetQueueDetailsForGarage(carParkId);
                return details != null ? Ok(details) : NotFound(new { message = $"Inga detaljer hittades för CarParkId {carParkId}." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ett fel uppstod vid hämtning av ködetaljer.", error = ex.Message });
            }
        }
    }
}
