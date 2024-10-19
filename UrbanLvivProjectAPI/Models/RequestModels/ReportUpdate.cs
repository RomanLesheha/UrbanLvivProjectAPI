namespace UrbanLvivProjectAPI.Models.RequestModels;

public class ReportUpdate
{
    public string title { get; set; }
    public string description { get; set; }
    public string image { get; set; }
    public int typeOfProblem { get; set; }
    public string location { get; set; }
}