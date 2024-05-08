namespace TTP_Web_api_Sample.Models
{
    public class BiWeeklyPeriodStatisticsUS
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int WrittenCount { get; set; }
        public int CompletedCount { get; set; }
        public int IncompletedCount { get; set; }
    }
}


