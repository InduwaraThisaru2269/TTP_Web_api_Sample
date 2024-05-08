using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TTP_Web_api_Sample.Models;
using TTP_Web_api_Sample.Services;

namespace TTP_Web_api_Sample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberActivityController : ControllerBase
    {

        public readonly JiraService _jiraService;

        public MemberActivityController(JiraService jiraService)
        {
            _jiraService = jiraService;
        }


        [HttpGet("/GetSprintDetails")]
        public async Task<ActionResult<List<SprintDetails>>> ShowsSprintDetails(int id)
        {
            try
            {
                List<SprintDetails> list = await _jiraService.GetSprintDetails(id);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("/GetMemberDetails")]
        public async Task<ActionResult<Dictionary<string, string>>> ShowMemberDetails(string projectKey)
        {
            try
            {
                Dictionary<string, string> list = await _jiraService.GetMembers(projectKey);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("/GetIssuesWithMembers")]
        public async Task<ActionResult<Dictionary<string, Dictionary<string, List<IssueData>>>>> ShowSprintWiseMemberIssues(string projectKey, int boardId)
        {
            try
            {
                Dictionary<string, Dictionary<string, List<IssueData>>> list = await _jiraService.GetSprintIssuesWithMembers(projectKey, boardId);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpGet("/GetTeamProductivity")]
        public async Task<ActionResult<Dictionary<string, TeamProductivity>>> ShowTeamProductivity(string projectKey, int boardId)
        {
            try
            {
                Dictionary<string, TeamProductivity> list = await _jiraService.CalculateTeamProductivityBySprint(projectKey, boardId);
                return Ok(list);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
