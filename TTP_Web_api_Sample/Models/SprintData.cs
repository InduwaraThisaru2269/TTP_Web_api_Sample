namespace TTP_Web_api_Sample.Models
{
    public class SprintData
    {
        public string Name { get; set; }

        public Dictionary<string, IssueData> Issues { get; set; }
    }
}
