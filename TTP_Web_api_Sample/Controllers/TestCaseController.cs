using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TTP_Web_api_Sample.Models;
using TTP_Web_api_Sample.Services;

namespace TTP_Web_api_Sample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestCaseController : ControllerBase
    {
        private readonly AIOTestsService _aIOTestsService;
        private readonly TestCaseStatisticsService _testCaseStatisticsService;

        public TestCaseController(AIOTestsService aIOTestsService, TestCaseStatisticsService testCaseStatisticsService)
        {
            _aIOTestsService = aIOTestsService;
            _testCaseStatisticsService = testCaseStatisticsService;

        }


        [HttpGet("/TestCasesForProject")]
        public async Task<ActionResult<List<TestCase>>> GetTestCasesInaProject(string projectKey)
        {
            try
            {
                List<TestCase> testCases = await _aIOTestsService.GetTestCaseDetails(projectKey);
                return testCases;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("/BiWeeklyTestCaseStats")]
        public async Task<ActionResult<List<BiWeeklyTestCaseStatistics>>> BiWeeklyTestCaseStats(DateTime startDate, string projectKey)
        {
            try
            {
                List<BiWeeklyTestCaseStatistics> stats = await _testCaseStatisticsService.CalculateBiWeeklyTestCaseStats(startDate, projectKey);
                return Ok(stats);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
