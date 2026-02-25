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

        // A-1 Registrations by Channel
        // GET api/helper/registrations-by-channel
        [HttpGet("registrations-by-channel")]
        public async Task<IActionResult> GetRegistrationsByChannel()
        {
            var result = await _repo.GetRegistrationsByChannelAsync();
            return Ok(result);
        }

        // A-3 Growth metrics
        // GET api/helper/registrations-growth?from=yyyy-MM-dd&to=yyyy-MM-dd
        [HttpGet("registrations-growth")]
        public async Task<IActionResult> GetRegistrationsGrowth([FromQuery] string from = null, [FromQuery] string to = null)
        {
            DateTime tTo = string.IsNullOrEmpty(to) ? DateTime.UtcNow : DateTime.Parse(to);
            DateTime tFrom = string.IsNullOrEmpty(from) ? tTo.AddDays(-30) : DateTime.Parse(from);
            var result = await _repo.GetRegistrationsGrowthAsync(tFrom, tTo);
            return Ok(result);
        }

        // A-2 Deep-dive by entity (filter utm_source contains)
        // GET api/helper/deep-dive?contains=university&top=20
        [HttpGet("deep-dive")]
        public async Task<IActionResult> GetDeepDive([FromQuery] string contains = "", [FromQuery] int top = 20)
        {
            var result = await _repo.GetDeepDiveByEntityAsync(contains, top);
            return Ok(result);
        }

        // B-1 Areas of Interest
        // GET api/helper/areas-of-interest
        [HttpGet("areas-of-interest")]
        public async Task<IActionResult> GetAreasOfInterest()
        {
            var result = await _repo.GetAreasOfInterestAsync();
            return Ok(result);
        }

        // B-2 Motivations
        // GET api/helper/motivations
        [HttpGet("motivations")]
        public async Task<IActionResult> GetMotivations()
        {
            var result = await _repo.GetMotivationsAsync();
            return Ok(result);
        }

        // B-3 Challenges
        // GET api/helper/challenges
        [HttpGet("challenges")]
        public async Task<IActionResult> GetChallenges()
        {
            var result = await _repo.GetChallengesAsync();
            return Ok(result);
        }

        // C-1 Channel effectiveness by region
        // GET api/helper/channel-effectiveness
        [HttpGet("channel-effectiveness")]
        public async Task<IActionResult> GetChannelEffectiveness()
        {
            var result = await _repo.GetChannelEffectivenessByRegionAsync();
            return Ok(result);
        }

        // C-2 Region distribution / gap analysis
        // GET api/helper/region-distribution
        [HttpGet("region-distribution")]
        public async Task<IActionResult> GetRegionDistribution()
        {
            var result = await _repo.GetRegionDistributionAsync();
            return Ok(result);
        }
    }
}