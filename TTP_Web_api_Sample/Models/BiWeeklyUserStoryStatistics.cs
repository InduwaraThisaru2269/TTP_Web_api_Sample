namespace TTP_Web_api_Sample.Models
{
    public class BiWeeklyUserStoryStatistics
    {
        public List<BiWeeklyPeriodStatisticsUS> PeriodStatistics { get; set; }

        public BiWeeklyUserStoryStatistics()
        {
            PeriodStatistics = new List<BiWeeklyPeriodStatisticsUS>();
        }

        public void AddBiWeeklyStatistics(DateTime startDate, DateTime endDate, int writtenCount, int completedCount, int incompletedCount)
        {
            PeriodStatistics.Add(new BiWeeklyPeriodStatisticsUS
            {
                StartDate = startDate,
                EndDate = endDate,
                WrittenCount = writtenCount,
                CompletedCount = completedCount,
                IncompletedCount = incompletedCount
                
            });
        }
    }
}
