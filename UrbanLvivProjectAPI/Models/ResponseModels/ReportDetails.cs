namespace UrbanLvivProjectAPI.Models.RequestModels;

public class ReportDetails
{
    public int id { get; set; }
    public string title { get; set; }
    public string description { get; set; }
    public string imageUrl { get; set; }
    public string typeOfProblem { get; set; }
    public double creatorId { get; set; }
    public string location { get; set; }
    public string timeOfCreation { get; set; }
    public int priority { get; set; }
    public bool isDone { get; set; }
}