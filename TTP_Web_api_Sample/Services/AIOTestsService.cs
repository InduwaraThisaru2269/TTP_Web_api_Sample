using Newtonsoft.Json;
using RestSharp;
using TTP_Web_api_Sample.Models;

namespace TTP_Web_api_Sample.Services
{
    public class AIOTestsService
    {
        private readonly RestClient _restClient;
       
        public AIOTestsService()
        {
            _restClient = new RestClient("https://tcms.aiojiraapps.com/aio-tcms/api/v1");
        }


        public async Task<List<TestCase>> GetTestCaseDetails(string projectKey)
        {
            List<TestCase> testCases = new List<TestCase>();

            int startAt = 0;

            while(true)
            {
                var request = new RestRequest($"/project/{projectKey}/testcase?startAt={startAt}", Method.Get);
                request.AddHeader("Authorization", "AioAuth ZGIzMzRmMjktNzIzZi0zMzlmLTg0YWEtNmU2NDJhZjYwMGQyLmMwNDAyNGQ2LTYyYjUtNGE5NS04MmYyLTI4N2JlMTM0YmMxYg==");

                dynamic response = await _restClient.ExecuteAsync(request);

                if(response.IsSuccessful)
                {
                    dynamic jsonResponse = JsonConvert.DeserializeObject(response.Content);

                    if(jsonResponse != null)
                    {
                        var items = jsonResponse?.items;

                        if(items != null)
                        {
                            foreach (var item in items)
                            {
                                long createdDateUnix = long.Parse(item.createdDate.ToString());

                                if(createdDateUnix > DateTimeOffset.MinValue.ToUnixTimeMilliseconds() && createdDateUnix < DateTimeOffset.MaxValue.ToUnixTimeMilliseconds())
                                {
                                    DateTime createdDate = DateTimeOffset.FromUnixTimeMilliseconds(createdDateUnix).UtcDateTime;
                                        
                                    testCases.Add(new TestCase
                                    {
                                        Id = item.ID,
                                        Name = item.key,
                                        CreatedDate = createdDate
                                    });
                                       
                                        
                                }
                            }
                        }

                    }

                    if (jsonResponse?.isLast == true)
                    {
                        break;
                    }

                    startAt += (int)jsonResponse?.maxResults;

                }
            }

            return testCases;
        }




    }
}
