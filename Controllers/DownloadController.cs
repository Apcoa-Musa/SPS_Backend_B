using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System;

namespace GarageQueueDownload.Controllers
{
    [ApiController]
    [Route("api/download")]
    public class DownloadController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DownloadController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("{dsNumber}")]
        public async Task<IActionResult> DownloadFile(int dsNumber)
        {
            if (dsNumber <= 0)
            {
                return BadRequest(new { message = "DSNumber �r obligatoriskt och m�ste vara st�rre �n 0." });
            }

            try
            {
                // H�mta filinformation via API
                var httpClient = _httpClientFactory.CreateClient();
                var apiResponse = await httpClient.GetAsync($"http://localhost:5265/api/files/{dsNumber}");

                if (!apiResponse.IsSuccessStatusCode)
                {
                    return NotFound(new { message = $"Ingen information hittades f�r DS {dsNumber} via API." });
                }

                var filePath = await apiResponse.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(filePath))
                {
                    return NotFound(new { message = "Filv�gen �r tom eller saknas i API-svaret." });
                }

                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound(new { message = $"Filen f�r DS {dsNumber} hittades inte p� servern." });
                }

                // Returnera filen
                var memory = new MemoryStream();
                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;

                return File(memory, "application/octet-stream", $"ds_{dsNumber}.txt");
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new
                {
                    message = "Kunde inte ansluta till API.",
                    error = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Ett ov�ntat fel uppstod vid nedladdning av filen.",
                    error = ex.Message
                });
            }
        }
    }
}
