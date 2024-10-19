using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UrbanLvivProjectAPI.Interfaces;
using UrbanLvivProjectAPI.Models;
using UrbanLvivProjectAPI.Models.RequestModels;

namespace UrbanLvivProjectAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : Controller
{
    
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    [HttpPost("UserLogin")]
    public async Task<IActionResult> UserLogin(UserLogin user)
    {
        ServerResponse result = await _authService.UserLogIn(user);
        return Ok(JsonConvert.SerializeObject(result));
    }
    
    [HttpPost("UserRegister")]
    public async Task<IActionResult> UserRegister(UserRegister user)
    {
        ServerResponse result = await _authService.UserRegister(user);
        return Ok(JsonConvert.SerializeObject(result));
    }
}