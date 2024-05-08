using Newtonsoft.Json;
using RestSharp;
using TTP_Web_api_Sample.Models;

namespace TTP_Web_api_Sample.Services
{
    public class GitHubService
    {
        private readonly RestClient _restClient;

        public GitHubService()
        {
            _restClient = new RestClient("https://api.github.com");
        }


        public async Task<List<PullRequest>> GetRaisedPRs(string repo)
        {
            List<PullRequest> PRs = new List<PullRequest>();

            
            int page = 1;
            int perPage = 100;

            while(true) 
            {
                var request = new RestRequest($"repos/inivossl/{repo}/pulls", Method.Get)
                    .AddQueryParameter("state", "all")
                    .AddQueryParameter("per_page", perPage.ToString())
                    .AddQueryParameter("page", page.ToString())
                    .AddHeader("Authorization", "bearer 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");


                dynamic response = await _restClient.ExecuteAsync(request);

                if (response.IsSuccessful)
                {
                    dynamic jsonresponse = JsonConvert.DeserializeObject(response.Content);

                    if (jsonresponse != null && jsonresponse.Count > 0)
                    {
                        foreach (var pr in jsonresponse)
                        {
                            PullRequest pullRequest = new PullRequest
                            {
                                Id = pr.id,
                                CreatedDate = DateTime.Parse(pr.created_at.ToString()),
                                MergedDate = pr.merged_at != null ? DateTime.Parse(pr.merged_at.ToString()) : null
                            };

                            PRs.Add(pullRequest);
                        }

                        page++;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return PRs;
        }


        public async Task<List<PullRequest>> GetPrsInAllRepos(List<string> repos)
        {
            List<PullRequest> allPullRequests = new List<PullRequest>();

            foreach (string repo in repos)
            {
                List<PullRequest> pullRequestsInRepo = await GetRaisedPRs(repo);
                allPullRequests.AddRange(pullRequestsInRepo);
            }

            return allPullRequests;
        }


        public async Task<List<PullRequestData>> GetBiWeeklyMergedAndCreatedCount(DateTime projectStartDate, List<string> repos)
        {
            List<PullRequest> pullRequests = await GetPrsInAllRepos(repos);

            //DateTime startDate = projectStartDate.Date;
            DateTime endDate = projectStartDate.AddDays(14).Date;

            List<PullRequestData> biWeeklyCounts = new List<PullRequestData>();

            while(endDate <= DateTime.UtcNow)
            {
                PullRequestData pullRequestData = new PullRequestData
                {
                    startDate = projectStartDate,
                    endDate = endDate,
                };

                foreach (var pr in pullRequests)
                {
                    if(pr.CreatedDate >= projectStartDate && pr.CreatedDate < endDate)
                    {
                        pullRequestData.CreatedCount++;
                    }
                    if (pr.MergedDate != null && pr.MergedDate >= projectStartDate && pr.MergedDate < endDate)
                    {
                        pullRequestData.MergedCount++;
                    }
                }

                biWeeklyCounts.Add(pullRequestData);

                projectStartDate = endDate;
                endDate = endDate.AddDays(14).Date;
                
            }
            
            return biWeeklyCounts;
        }







        public async Task<Dictionary<string, string>> GetOverallBuiltStatus(List<string> repos)
        {
            
            Dictionary<string, string> buildStatuses = new Dictionary<string, string>();

            foreach (var repo in repos)
            {
                var request = new RestRequest($"repos/inivossl/{repo}/actions/runs", Method.Get)
                .AddHeader("Authorization", "bearer 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");

                dynamic response = await _restClient.ExecuteAsync(request);

                if (response.IsSuccessful)
                {
                    dynamic jsonResponse = JsonConvert.DeserializeObject(response.Content);

                    if (jsonResponse != null)
                    {
                        if ((int)jsonResponse.total_count == 0)
                        {
                            buildStatuses.Add(repo, "This Project has no runs.");

                        }

                        else
                        {
                            string status = jsonResponse.workflow_runs[0].conclusion;
                            buildStatuses.Add(repo, status);
                        }
                    }
                }
            }
            
            return buildStatuses;
        }


    }
}
