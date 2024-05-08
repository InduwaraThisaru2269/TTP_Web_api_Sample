using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TTP_Web_api_Sample.Models;
using TTP_Web_api_Sample.Services;

namespace TTP_Web_api_Sample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PRController : ControllerBase
    {
        private readonly GitHubService _gitHubService;

        public PRController(GitHubService gitHubService)
        {
            _gitHubService = gitHubService;
        }


        [HttpGet("/GetNumberOfPRs")]
        public async Task<ActionResult<List<PullRequest>>> ShowPRs(string repo)
        {
            try
            {
                List<PullRequest> prs = await _gitHubService.GetRaisedPRs(repo);
                return Ok(prs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpGet("/totalPRs")]
        public async Task<ActionResult<List<PullRequest>>> GetTotalPRsInRepos([FromQuery] List<string> repos)
        {
            if (repos == null || repos.Count == 0)
            {
                return BadRequest("List of repositories cannot be empty");
            }

            try
            {
                List<PullRequest> totalPRs = await _gitHubService.GetPrsInAllRepos(repos);
                return Ok(totalPRs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }




        [HttpGet("/GetCreatedAndMergedCount")]
        public async Task<ActionResult<List<PullRequestData>>> ShowCreatedAndMergedCount(DateTime projectStartDate, [FromQuery] List<string> repos)
        {
            try
            {
                List<PullRequestData> data = await _gitHubService.GetBiWeeklyMergedAndCreatedCount(projectStartDate, repos);
                return Ok(data);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
