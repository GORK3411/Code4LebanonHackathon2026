using Code4LebanonApi.Services;
using Microsoft.AspNetCore.Mvc;

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
            var data = await _service.GetDataAsync();
            return Ok(data);
        }
    }
}
