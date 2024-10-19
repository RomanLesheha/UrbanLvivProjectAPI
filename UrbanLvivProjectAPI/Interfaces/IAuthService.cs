using UrbanLvivProjectAPI.Models;
using UrbanLvivProjectAPI.Models.RequestModels;

namespace UrbanLvivProjectAPI.Interfaces;

public interface IAuthService
{
    Task<ServerResponse> UserLogIn(UserLogin user);
    
    Task<ServerResponse> UserRegister(UserRegister user);
}