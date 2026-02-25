using Code4LebanonApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace Code4LebanonApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AController : ControllerBase
    {
        private readonly NumuSurveyService _service;

        public AController(NumuSurveyService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            DateTime startDate = DateTime.ParseExact("2026-02-01", "yyyy-MM-dd", CultureInfo.InvariantCulture);
            DateTime endDate = DateTime.ParseExact("2026-02-28", "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var data = await _service.GetLastResponseDateAsync();
            return Ok(data);
        }
    }
}
