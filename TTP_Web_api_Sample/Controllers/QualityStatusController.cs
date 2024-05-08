using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TTP_Web_api_Sample.Services;

namespace TTP_Web_api_Sample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QualityStatusController : ControllerBase
    {
        private readonly GitHubService _gitHubService;
        private readonly SonarcloudService _sonarcloudService;

        public QualityStatusController(GitHubService gitHubService, SonarcloudService sonarcloudService)
        {
            _gitHubService = gitHubService;
            _sonarcloudService = sonarcloudService;
        }


        [HttpGet("/GetBuildStatus")]
        public async Task<ActionResult<Dictionary<string, string>>> ShowBuildStatus([FromQuery] List<string> repos)
        {
            try
            {
                Dictionary<string, string> list = await _gitHubService.GetOverallBuiltStatus(repos);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpGet("/GetQualityGate")]
        public async Task<ActionResult<Dictionary<string, string>>> ShowQualityGate([FromQuery] List<string> repoKeys)
        {
            try
            {
                Dictionary<string, string> list = await _sonarcloudService.GetQualityGateStatuses(repoKeys);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
