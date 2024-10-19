namespace UrbanLvivProjectAPI.Models.GeneralModels;

public class User
{
    public int UserId { get; set; }
    public string UserFirstName { get; set; }
    public string UserLastName { get; set; }
    public string UserEmail { get; set; }
    public string UserPhoneNumber { get; set; }
    public string UserPassword { get; set; }
}