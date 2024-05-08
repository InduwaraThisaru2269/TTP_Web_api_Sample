using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using TTP_Web_api_Sample.Models;


namespace TTP_Web_api_Sample.Services
{
    public class JiraService 
    {

        private readonly RestClient _restClient;

        public JiraService()
        {
            _restClient = new RestClient("https://inivos.atlassian.net");
        }


        public async Task<List<string>> GetProjectKeys()
        {
            List<string> ProjectKeys = new List<string>();

            int startAt = 0;

            while (true)
            {
                var request = new RestRequest($"rest/agile/1.0/board?startAt={startAt}", Method.Get);

                string username = "induwara.thisaru@inivosglobal.com";
                string password = "ATATT3xFfGF0FSsxJg3UmgZHqoS7KJeuXpjdUpjcIgfRFvd7gPWg7Vo4L2iit-kv9J6E37bzQCoXi4QdZze8xMawI0YdAiJe6AAu3tc-kHb6oTiAi7O8lgGu_bTi_wlK-Y-rtQ6-VNfspTocT1D6Cp7_0drAXEWgsh4hABOTTGCYK_7MKrC1rJc=FDA38671";

                request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}")));

                dynamic response = await _restClient.ExecuteAsync(request);

                if (response.IsSuccessful)
                {
                    dynamic jsonResponse = JsonConvert.DeserializeObject(response.Content);

                    if(jsonResponse != null && jsonResponse.values != null)
                    {
                        var values = jsonResponse?.values;

                        foreach (var value in values)
                        {
                            string ProjectKey = value?.location.projectKey;

                            if (!string.IsNullOrEmpty(ProjectKey))
                            {
                                ProjectKeys.Add(ProjectKey);
                            }
                        }

                        if (jsonResponse?.isLast == true)
                        {
                            break;
                        }

                        startAt += (int)jsonResponse?.values.Count;
                    }
                    
                }
                
            }

            return ProjectKeys;
        }




        public async Task<List<string>> GetProjectNames()
        {
            List<string> ProjectNames = new List<string>();

            int startAt = 0;

            while (true)
            {
                var request = new RestRequest($"rest/agile/1.0/board?startAt={startAt}", Method.Get);

                string username = "induwara.thisaru@inivosglobal.com";
                string password = "ATATT3xFfGF0FSsxJg3UmgZHqoS7KJeuXpjdUpjcIgfRFvd7gPWg7Vo4L2iit-kv9J6E37bzQCoXi4QdZze8xMawI0YdAiJe6AAu3tc-kHb6oTiAi7O8lgGu_bTi_wlK-Y-rtQ6-VNfspTocT1D6Cp7_0drAXEWgsh4hABOTTGCYK_7MKrC1rJc=FDA38671";

                request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}")));

                dynamic response = await _restClient.ExecuteAsync(request);

                if (response.IsSuccessful)
                {
                    dynamic jsonResponse = JsonConvert.DeserializeObject(response.Content);

                    if (jsonResponse != null && jsonResponse.values != null)
                    {
                        var values = jsonResponse?.values;

                        foreach (var value in values)
                        {
                            string ProjectName = value?.location.displayName;

                            if (!string.IsNullOrEmpty(ProjectName))
                            {
                                ProjectNames.Add(ProjectName);
                            }
                        }

                        if (jsonResponse?.isLast == true)
                        {
                            break;
                        }

                        startAt += (int)jsonResponse?.values.Count;
                    }

                }

            }

            return ProjectNames;
        }



        public async Task<List<UserStory>> ShowUserStoriesInProject(string projectKey)
        {
            int startAt = 0;
            List<UserStory> stories = new List<UserStory>();

            while (true)
            {
                var request = new RestRequest("/rest/api/3/search", Method.Get);

                string jqlQuery = $"issuetype = Story AND project = {projectKey} ";
                string username = "induwara.thisaru@inivosglobal.com";
                string password = "ATATT3xFfGF0FSsxJg3UmgZHqoS7KJeuXpjdUpjcIgfRFvd7gPWg7Vo4L2iit-kv9J6E37bzQCoXi4QdZze8xMawI0YdAiJe6AAu3tc-kHb6oTiAi7O8lgGu_bTi_wlK-Y-rtQ6-VNfspTocT1D6Cp7_0drAXEWgsh4hABOTTGCYK_7MKrC1rJc=FDA38671";

                request.AddQueryParameter("jql", jqlQuery);
                request.AddQueryParameter("maxResults", 100);
                request.AddQueryParameter("startAt", startAt);
                request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}")));

                dynamic response = await _restClient.ExecuteAsync(request);


                if (response.IsSuccessful)
                {
                    dynamic jsonResponse = JsonConvert.DeserializeObject(response.Content);

                    if (jsonResponse != null)
                    {
                        var issues = jsonResponse.issues;

                        foreach (var issue in issues)
                        {
                            if (issue != null)
                            {
                                stories.Add(new UserStory
                                {
                                    Id = issue.id,
                                    StoryName = issue.key,
                                    CreatedDate = DateTime.Parse(issue.fields.created.ToString()),
                                    Status = issue.fields.status.name,
                                });
                            }

                        }
                    }

                    startAt += (int)jsonResponse?.issues.Count;

                    if (startAt >= (int)jsonResponse.total)
                    {
                        break;
                    }


                }
            }

            return stories;
        }




        // This is for getting total time estimate vs Total time spent in a sprint in a project


        public async Task<Dictionary<int, string>> GetProjectKeysANDBoardIds()
        {
            Dictionary<int, string> boardIdList = new Dictionary<int, string>();
            int startAt = 0;

            while (true)
            {
                var request = new RestRequest($"rest/agile/1.0/board?startAt={startAt}", Method.Get);

                string username = "induwara.thisaru@inivosglobal.com";
                string password = "ATATT3xFfGF0FSsxJg3UmgZHqoS7KJeuXpjdUpjcIgfRFvd7gPWg7Vo4L2iit-kv9J6E37bzQCoXi4QdZze8xMawI0YdAiJe6AAu3tc-kHb6oTiAi7O8lgGu_bTi_wlK-Y-rtQ6-VNfspTocT1D6Cp7_0drAXEWgsh4hABOTTGCYK_7MKrC1rJc=FDA38671";

                request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}")));

                dynamic response = await _restClient.ExecuteAsync(request);

                if (response.IsSuccessful)
                {
                    dynamic jsonresponse = JsonConvert.DeserializeObject(response.Content);

                    if (jsonresponse == null)
                    {
                        throw new Exception("Failed to deserialize JSON response.");
                    }

                    var boards = jsonresponse.values;

                    foreach (var board in boards)
                    {
                        int boardId = board.id;
                        //string boardName = board.name;
                        string projectKey = board.location.projectKey;

                        if (projectKey != null)
                        {
                            boardIdList[boardId] = projectKey;
                        }
                        

                    }

                    if (jsonresponse.isLast == true)
                    {
                        break;
                    }

                    startAt += (int)jsonresponse.maxResults;
                }

                else
                {
                    throw new Exception($"Failed to retrieve data. Status code: {response.ErrorMessage}");
                }

            }

            return boardIdList;
        }






        public async Task<List<SprintDetails>> GetSprintDetails(int boardId)
        {
            List<SprintDetails> sprints = new List<SprintDetails>();

            int startAt = 0;

            while (true)
            {
                var request = new RestRequest($"/rest/agile/1.0/board/{boardId}/sprint?startAt={startAt}", Method.Get);
                string username = "induwara.thisaru@inivosglobal.com";
                string password = "ATATT3xFfGF0FSsxJg3UmgZHqoS7KJeuXpjdUpjcIgfRFvd7gPWg7Vo4L2iit-kv9J6E37bzQCoXi4QdZze8xMawI0YdAiJe6AAu3tc-kHb6oTiAi7O8lgGu_bTi_wlK-Y-rtQ6-VNfspTocT1D6Cp7_0drAXEWgsh4hABOTTGCYK_7MKrC1rJc=FDA38671"; // Consider using secure storage

                request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}")));

                dynamic response = await _restClient.ExecuteAsync(request);

                if (response.IsSuccessful)
                {
                    dynamic jsonResponse = JsonConvert.DeserializeObject(response.Content);

                    if (jsonResponse != null)
                    {
                        var values = jsonResponse.values;

                        if (values != null)
                        {
                            foreach (var v in values)
                            {
                                sprints.Add(new SprintDetails
                                {
                                    Id = v.id,
                                    SprintName = v.name,
                                    StartDate = v.startDate,
                                    EndDate = v.endDate
                                });
                            }
                        }
                    }

                    if(jsonResponse?.isLast == true)
                    {
                        break;
                    }

                    startAt = (int)jsonResponse?.values.Count;
                }



            }

            return sprints;

        }








        public async Task<Dictionary<string, string>> GetMembers(string projectKey)
        {

            Dictionary<string, string> members = new Dictionary<string, string>();  

            var request = new RestRequest($"rest/api/3/user/search/query", Method.Get);
            request.AddQueryParameter("query", $"is assignee of {projectKey}");

            string username = "induwara.thisaru@inivosglobal.com";
            string password = "ATATT3xFfGF0FSsxJg3UmgZHqoS7KJeuXpjdUpjcIgfRFvd7gPWg7Vo4L2iit-kv9J6E37bzQCoXi4QdZze8xMawI0YdAiJe6AAu3tc-kHb6oTiAi7O8lgGu_bTi_wlK-Y-rtQ6-VNfspTocT1D6Cp7_0drAXEWgsh4hABOTTGCYK_7MKrC1rJc=FDA38671"; // Consider using secure storage

            request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}")));

            dynamic response = await _restClient.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                dynamic jsonResponse = JsonConvert.DeserializeObject(response.Content);

                if(jsonResponse != null)
                {
                    var values = jsonResponse.values;

                    if (values != null)
                    {
                        foreach(var value in values)
                        {
                            string displayName = value.displayName?.ToString();
                            string accountId = value.accountId?.ToString();

                            if (!string.IsNullOrEmpty(displayName) && !string.IsNullOrEmpty(accountId))
                            {
                                // Add to dictionary
                                members[displayName] = accountId;
                            }
                        }
                    }
                }
            }

            return members;
        }





        public async Task<Dictionary<string, Dictionary<string, List<IssueData>>>> GetSprintIssuesWithMembers(string projectKey, int boardId)
        {
            Dictionary<string, string> members = await GetMembers(projectKey);

            List<SprintDetails> sprintDetails = await GetSprintDetails(boardId);

            Dictionary<string, Dictionary<string, List<IssueData>>> sprintMemberIssues = new Dictionary<string, Dictionary<string, List<IssueData>>>();
            
            

            foreach (var sprint in sprintDetails)
            {
                string sprintName = sprint.SprintName;
                Dictionary<string, List<IssueData>> sprintIssuesByMember = new Dictionary<string, List<IssueData>>();

                foreach (var member in members)
                {
                    string memberId = member.Value.ToString();
                    string memberName = member.Key.ToString();

                    List<IssueData> issueDetails = new List<IssueData>();
                    

                    var request = new RestRequest("rest/api/3/search", Method.Get);

                    string jql = $"assignee = {memberId} AND project = {projectKey} AND sprint = \"{sprintName}\"";
                    request.AddQueryParameter("jql", jql);

                    string username = "induwara.thisaru@inivosglobal.com";
                    string password = "ATATT3xFfGF0FSsxJg3UmgZHqoS7KJeuXpjdUpjcIgfRFvd7gPWg7Vo4L2iit-kv9J6E37bzQCoXi4QdZze8xMawI0YdAiJe6AAu3tc-kHb6oTiAi7O8lgGu_bTi_wlK-Y-rtQ6-VNfspTocT1D6Cp7_0drAXEWgsh4hABOTTGCYK_7MKrC1rJc=FDA38671"; // Consider using secure storage

                    request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}")));

                    dynamic response = await _restClient.ExecuteAsync(request);

                    if(response.IsSuccessful)
                    {
                        dynamic jsonResponse = JsonConvert.DeserializeObject(response.Content);    


                        if(jsonResponse != null)
                        {
                            var issues = jsonResponse.issues;

                            foreach (var issue in issues)
                            {
                                int Id = int.Parse(issue.id.ToString());   
                                string issueName = issue.key;
                                /*double? TimeEstimate = (issue.fields.aggregatetimeoriginalestimate) / 3600;
                                double? TimeSpent = (issue.fields.aggregatetimespent)/ 3600;*/
                                double? TimeEstimate = issue.fields.aggregatetimeoriginalestimate != null ? ((double)issue.fields.aggregatetimeoriginalestimate) / 3600 : null;
                                double? TimeSpent = issue.fields.aggregatetimespent != null ? ((double)issue.fields.aggregatetimespent) / 3600 : null;

                                /*if (issue.fields.aggregatetimeoriginalestimate == null && issue.fields.aggregatetimespent == null)
                                {
                                    TimeEstimate = 0;
                                    TimeSpent = 0;
  
                                }*/

                                issueDetails.Add(new IssueData
                                { 
                                    Id = Id,
                                    Key = issueName,
                                    TimeEstimate = TimeEstimate,
                                    TimeSpent = TimeSpent,
                                });

                           
                                
                                

                            }
                        }
                    }

                    sprintIssuesByMember.Add(memberName, issueDetails); 
                }

                sprintMemberIssues.Add(sprintName, sprintIssuesByMember);

            }

            return sprintMemberIssues;
        }






        public async Task<Dictionary<string, TeamProductivity>> CalculateTeamProductivityBySprint(string projectKey, int boardId)
        {
            Dictionary<string, Dictionary<string, List<IssueData>>> sprintMemberIssues = await GetSprintIssuesWithMembers(projectKey, boardId);

            Dictionary<string, TeamProductivity> sprintTeamProductivity = new Dictionary<string, TeamProductivity>();

            foreach(var sprint in sprintMemberIssues)
            {
                string sprintName = sprint.Key;
                double totalEstimate = 0;
                double totalSpent = 0;

                foreach (var memberIssues in sprint.Value)
                {
                    foreach (var issue in memberIssues.Value)
                    {
                        if (issue.TimeEstimate.HasValue)
                        {
                            totalEstimate += issue.TimeEstimate.Value;
                        }

                        if (issue.TimeSpent.HasValue)
                        {
                            totalSpent += issue.TimeSpent.Value;
                        }
                    }
                    
                }

                TeamProductivity sprintProductivity = new TeamProductivity
                {
                    TimeEstimate = totalEstimate,
                    TimeSpent = totalSpent,
                };

                sprintTeamProductivity.Add(sprintName, sprintProductivity);
            }

            return sprintTeamProductivity;
        }













    }
}
