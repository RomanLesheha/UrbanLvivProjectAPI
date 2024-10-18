using FireSharp;
using FireSharp.Config;
using UrbanLvivProjectAPI.Interfaces;

namespace UrbanLvivProjectAPI.Services;

public class FirebaseConnectingService : IFirebaseConnectingService
{
    private readonly IConfiguration _configuration;
    
    public FirebaseConnectingService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public FirebaseClient GetFirebaseClient()
    {
        var firebaseSection = _configuration.GetSection("Firebase");
        var config = new FirebaseConfig
        {
            AuthSecret = firebaseSection["AuthSecret"],
            BasePath = firebaseSection["BasePath"]
        };

        return new FireSharp.FirebaseClient(config);
    }
}