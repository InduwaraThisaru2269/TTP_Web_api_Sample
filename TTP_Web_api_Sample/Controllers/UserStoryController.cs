using Microsoft.AspNetCore.Mvc;
using TTP_Web_api_Sample.Models;
using TTP_Web_api_Sample.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TTP_Web_api_Sample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserStoryController : ControllerBase
    {
        private readonly UserStoryStatisticsService _userStoryStatisticsService;
        private readonly JiraService _jiraService;

        public UserStoryController(UserStoryStatisticsService userStoryStatisticsService, JiraService jiraService)
        {
            _userStoryStatisticsService = userStoryStatisticsService;
            _jiraService = jiraService;
        }



        [HttpGet("/ProjectKeys&BoardIds")]
        public async Task<ActionResult<Dictionary<int, string>>> ProjectKeysAndBoardIds() 
        {
            try
            {
                Dictionary<int, string> list = await _jiraService.GetProjectKeysANDBoardIds();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpGet("/ProjectNames")]
        public async Task<IActionResult> GetProjectNames()
        {
            try
            {
                List<string> projectNames = await _jiraService.GetProjectNames();
                return Ok(projectNames);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("/GetUserStories")]
        public async Task<ActionResult<List<UserStory>>> GetBiweeklyActivity(string projectKey)
        {
            try
            {
                List<UserStory> stories = await _jiraService.ShowUserStoriesInProject(projectKey);
                return Ok(stories);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("/GetBiWeeklyUserStoryStats")]
        public async Task<ActionResult<BiWeeklyUserStoryStatistics>> GetBiweeklyActivity(DateTime startDate, string projectKey)
        {
            try
            {
                BiWeeklyUserStoryStatistics stories = await _userStoryStatisticsService.CalculateBiWeeklyuserStoryStats(startDate, projectKey);
                return Ok(stories);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
