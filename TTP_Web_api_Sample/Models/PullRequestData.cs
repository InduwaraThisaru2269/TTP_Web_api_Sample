namespace TTP_Web_api_Sample.Models
{
    public class PullRequestData
    {
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public int MergedCount { get; set; }
        public int CreatedCount { get; set; }
    }
}
