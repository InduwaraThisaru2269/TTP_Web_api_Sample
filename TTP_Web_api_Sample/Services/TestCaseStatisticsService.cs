using TTP_Web_api_Sample.Models;

namespace TTP_Web_api_Sample.Services
{
    public class TestCaseStatisticsService
    {
        private readonly AIOTestsService _aIOTestsService;

        public TestCaseStatisticsService(AIOTestsService aIOTestsService)
        {
            _aIOTestsService = aIOTestsService;
        }


        public async Task<List<BiWeeklyTestCaseStatistics>> CalculateBiWeeklyTestCaseStats(DateTime startDate, string projectKey)
        {
            List<TestCase> testcases = await _aIOTestsService.GetTestCaseDetails(projectKey);
            List<BiWeeklyTestCaseStatistics> biWeeklyStatistics = new List<BiWeeklyTestCaseStatistics>();

            DateTime currentDate = startDate.Date;
            DateTime endDate = DateTime.Now.Date;

            while (currentDate < endDate)
            {
                DateTime biWeeklyEndDate = currentDate.AddDays(14).Date;

                BiWeeklyTestCaseStatistics biWeeklyStats = new BiWeeklyTestCaseStatistics
                {
                    StartDate = currentDate,
                    EndDate = biWeeklyEndDate,
                    WrittenCount = 0,
                };

                foreach (var testcase in testcases)
                {
                    //DateTime createdDate = DateTimeOffset.FromUnixTimeMilliseconds(testcase.CreatedDate).UtcDateTime;
                    if(testcase.CreatedDate >= currentDate && testcase.CreatedDate < biWeeklyEndDate)
                    { 
                        biWeeklyStats.WrittenCount++;
                    }
                }

                biWeeklyStatistics.Add(biWeeklyStats);

                currentDate = biWeeklyEndDate;
            }

            return biWeeklyStatistics;
        }











    }


}
