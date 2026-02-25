using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Code4LebanonApi.Services;

namespace Code4LebanonApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HelperController : ControllerBase
    {
        private readonly ILogger<HelperController> _logger;
        private readonly Code4LebanonRepository _repo;

        public HelperController(ILogger<HelperController> logger, Code4LebanonRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }

        // GET api/helper/applicants-per-region
        [HttpGet("applicants-per-region")]
        public async Task<IActionResult> GetApplicantsPerRegion()
        {
            var result = await _repo.GetApplicantsPerRegionAsync();
            return Ok(result);
        }

        // GET api/helper/published-surveys
        [HttpGet("published-surveys")]
        public async Task<IActionResult> GetPublishedSurveys()
        {
            var result = await _repo.GetPublishedSurveysAsync();
            return Ok(result);
        }

        // GET api/helper/active-published-surveys
        [HttpGet("active-published-surveys")]
        public async Task<IActionResult> GetActivePublishedSurveys()
        {
            var result = await _repo.GetActivePublishedSurveysAsync();
            return Ok(result);
        }

        // GET api/helper/responses/{surveyId}
        [HttpGet("responses/{surveyId}")]
        public async Task<IActionResult> GetResponsesForSurvey(string surveyId)
        {
            if (string.IsNullOrEmpty(surveyId)) return BadRequest("surveyId required");
            var result = await _repo.GetResponsesForSurveyAsync(surveyId);
            return Ok(result);
        }
    }
}