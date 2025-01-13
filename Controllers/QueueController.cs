using GarageQueueUpload.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

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
            if (!Guid.TryParse(id, out Guid parsedId))
            {
                return BadRequest(new { message = "Id m�ste vara ett giltigt Guid." });
            }

            try
            {
                var queue = await _carParksApiService.GetQueueById(parsedId);
                return queue != null ? Ok(queue) : NotFound(new { message = $"Ingen k� hittades med ID {id}." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ett fel uppstod vid h�mtning av k�data.", error = ex.Message });
            }
        }

        [HttpGet("GetByCarParkId/{carParkId}")]
        public async Task<IActionResult> GetByCarParkId(string carParkId)
        {
            if (!Guid.TryParse(carParkId, out Guid parsedCarParkId))
            {
                return BadRequest(new { message = "CarParkId m�ste vara ett giltigt Guid." });
            }

            try
            {
                var queue = await _carParksApiService.GetQueueByCarParkId(parsedCarParkId);
                return queue != null ? Ok(queue) : NotFound(new { message = $"Ingen k� hittades f�r CarParkId {carParkId}." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ett fel uppstod vid h�mtning av k�data.", error = ex.Message });
            }
        }

        [HttpGet("GetQueueDetailsForGarage/{carParkId}")]
        public async Task<IActionResult> GetQueueDetailsForGarage(string carParkId)
        {
            if (!Guid.TryParse(carParkId, out Guid parsedCarParkId))
            {
                return BadRequest(new { message = "CarParkId m�ste vara ett giltigt Guid." });
            }

            try
            {
                var details = await _carParksApiService.GetQueueDetailsForGarage(parsedCarParkId);
                return details != null ? Ok(details) : NotFound(new { message = $"Inga detaljer hittades f�r CarParkId {carParkId}." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ett fel uppstod vid h�mtning av k�detaljer.", error = ex.Message });
            }
        }
    }
}
