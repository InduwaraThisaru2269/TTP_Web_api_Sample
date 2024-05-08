using Newtonsoft.Json;
using TTP_Web_api_Sample.Models;

namespace TTP_Web_api_Sample.Services
{
    public class UserStoryStatisticsService
    {
        private readonly JiraService _jiraService;

        public UserStoryStatisticsService(JiraService jiraService)
        {
            _jiraService = jiraService;
        }



        public async Task<BiWeeklyUserStoryStatistics> CalculateBiWeeklyuserStoryStats(DateTime projectStartDate, string projectKey)
        {
            List<UserStory> userStories = await _jiraService.ShowUserStoriesInProject(projectKey);

            var biWeeklyStatistics = new BiWeeklyUserStoryStatistics();
            DateTime currentDate = projectStartDate.Date;
            DateTime endDate = DateTime.Now.Date; 

            var biWeeklyPeriodStatistics = new Dictionary<DateTime, BiWeeklyPeriodStatisticsUS>();

            while (currentDate < endDate)
            {
                DateTime biWeeklyEndDate = currentDate.AddDays(14).Date;

                biWeeklyPeriodStatistics[currentDate] = new BiWeeklyPeriodStatisticsUS
                {
                    StartDate = currentDate,
                    EndDate = biWeeklyEndDate,
                    WrittenCount = 0,
                    CompletedCount = 0,
                    IncompletedCount = 0
                };

                currentDate = biWeeklyEndDate;
            }

            foreach (var userStory in userStories)
            {
                foreach (var biWeeklyPeriod in biWeeklyPeriodStatistics.Values)
                {
                    if (userStory.CreatedDate >= biWeeklyPeriod.StartDate && userStory.CreatedDate <= biWeeklyPeriod.EndDate)
                    {
                        biWeeklyPeriod.WrittenCount++;

                        if (userStory.Status.Equals("Done"))
                        {
                            biWeeklyPeriod.CompletedCount++;

                            biWeeklyPeriod.IncompletedCount = biWeeklyPeriod.WrittenCount - biWeeklyPeriod.CompletedCount;
                        }
                    }
                }
            }

            biWeeklyStatistics.PeriodStatistics.AddRange(biWeeklyPeriodStatistics.Values);

            return biWeeklyStatistics;

        }

    }
}
