using FireSharp;
using FireSharp.Config;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UrbanLvivProjectAPI.Interfaces;

namespace UrbanLvivProjectAPI.Services;

public class FirebaseConnectingService : IFirebaseConnectingService
{
    public string GetApiKey()
    {
        string configFile = "Configs/firebaseConfig.json";
        string configJson = File.ReadAllText(configFile);
        var config = JObject.Parse(configJson);
        return config["API_KEY"].ToString();
    }

    public FirebaseClient GetFirebaseClient()
    {
        var firebaseConfigPath = "Configs/firebaseConfig.json";
        var configJson = System.IO.File.ReadAllText(firebaseConfigPath);
        var config = JsonConvert.DeserializeObject<FirebaseConfig>(configJson);

        return new FireSharp.FirebaseClient(config);
    }
}