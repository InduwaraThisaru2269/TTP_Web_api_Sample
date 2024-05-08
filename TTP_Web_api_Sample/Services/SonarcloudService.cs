using Newtonsoft.Json;
using RestSharp;

namespace TTP_Web_api_Sample.Services
{
    public class SonarcloudService
    {
        private readonly RestClient _restClient;

        public SonarcloudService()
        {
            _restClient = new RestClient("https://sonarcloud.io");
        }


        public async Task<Dictionary<string, string>> GetQualityGateStatuses(List<string> repoKeys)
        {
            Dictionary<string, string> statuses = new Dictionary<string, string>();

            foreach (string repoKey in repoKeys)
            {
                var request = new RestRequest($"/api/qualitygates/project_status?projectKey={repoKey}", Method.Get)
                    .AddHeader("Authorization", "Bearer 334c7249b4b9e71093fe5bc50fa0f56d51b3c47e");

                dynamic response = await _restClient.ExecuteAsync(request);

                if (response.IsSuccessful)
                {
                    dynamic jsonResponse = JsonConvert.DeserializeObject(response.Content);

                    if (jsonResponse != null)
                    {
                        string status = jsonResponse.projectStatus.status;
                        statuses.Add(repoKey, status);
                    }
   
                }
                else
                {
                    string status = "Not Configured";
                    statuses.Add(repoKey, status);
                }
            }

            return statuses;
        }
    }
}
