namespace UrbanLvivProjectAPI.Models.RequestModels;

public class ReportCreate
{
    public string title { get; set; }
    public string description { get; set; }
    public string image { get; set; }
    public int typeOfProblem { get; set; }
    public int creatorId { get; set; }
    public string location { get; set; }
}