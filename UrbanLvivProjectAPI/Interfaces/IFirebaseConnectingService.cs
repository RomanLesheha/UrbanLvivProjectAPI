using FireSharp;
namespace UrbanLvivProjectAPI.Interfaces;

public interface IFirebaseConnectingService
{
    public FirebaseClient GetFirebaseClient();
    
}