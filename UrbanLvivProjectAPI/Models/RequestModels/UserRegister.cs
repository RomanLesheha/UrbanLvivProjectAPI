namespace UrbanLvivProjectAPI.Models.RequestModels;

public class UserRegister
{
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string email { get; set; }
    public string phone { get; set; }
    public string password { get; set; }
}