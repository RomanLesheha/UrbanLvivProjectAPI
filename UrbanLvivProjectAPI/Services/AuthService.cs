using System.Text.RegularExpressions;
using UrbanLvivProjectAPI.Healpers;
using UrbanLvivProjectAPI.Interfaces;
using UrbanLvivProjectAPI.Models;
using UrbanLvivProjectAPI.Models.RequestModels;

namespace UrbanLvivProjectAPI.Services;

public class AuthService : IAuthService
{
    private readonly IFirebaseConnectingService _firebaseConnectingService;

    public AuthService(IFirebaseConnectingService firebaseConnectingService)
    {
        _firebaseConnectingService = firebaseConnectingService;
    }
    
    public async Task<ServerResponse> UserLogIn(UserLogin user)
    {
        if (string.IsNullOrEmpty(user.user_email) || string.IsNullOrEmpty(user.password))
        {
            return new ServerResponse("Email and password are required.", 400);
        }

        using (var client = _firebaseConnectingService.GetFirebaseClient())
        {
            if (client != null)
            {
                var response = await client.GetAsync("Users");
                if (response.Body != "null")
                {
                    var users = response.ResultAs<Dictionary<string, dynamic>>();
                    
                    var userExists = users.Values.FirstOrDefault(u => u.email == user.user_email);
                    if (userExists != null)
                    {
                        string storedPassword = userExists.password.ToString();
                        if (PasswordHelper.VerifyPassword(user.password, storedPassword))
                        {
                            return new ServerResponse
                            {
                                message = "User logged in successfully.",
                                statusCode = 200,
                                data = new { userEmail = userExists.email, isLogin = true }
                            };
                        }
                        else
                        {
                            return new ServerResponse("Invalid password.", 400);
                        }
                    }
                    else
                    {
                        return new ServerResponse("User not found.", 404);
                    }
                }
                else
                {
                    return new ServerResponse("No users found in the database.", 404);
                }
            }
            else
            {
                return new ServerResponse("Failed to connect to Firebase.", 400);
            }
        }
    }


   public async Task<ServerResponse> UserRegister(UserRegister user)
    {
        if (string.IsNullOrEmpty(user.firstName) || 
            string.IsNullOrEmpty(user.lastName) || 
            string.IsNullOrEmpty(user.email) || 
            string.IsNullOrEmpty(user.phone) || 
            string.IsNullOrEmpty(user.password))
        {
            return new ServerResponse("All fields are required.", 400);
        }
        
        if (!Regex.IsMatch(user.email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            return new ServerResponse("Invalid email format.", 400);
        }
        
        if (user.password.Length < 8)
        {
            return new ServerResponse("Password must be at least 8 characters long.", 400);
        }

        using (var client = _firebaseConnectingService.GetFirebaseClient())
        {
            if (client != null)
            {
                var usersFolderResponse = await client.GetAsync("Users");
                if (usersFolderResponse.Body == "null")
                {
                    await client.SetAsync("Users", new { });
                }

                Random rand = new Random();
                int userId = rand.Next(2000000, 999999999);
                
                var existingUserResponse = await client.GetAsync($"Users");
                if (existingUserResponse.Body != "null")
                {
                    var users = existingUserResponse.ResultAs<Dictionary<string, dynamic>>();
                    if (users.Values.Any(u => u.email == user.email))
                    {
                        return new ServerResponse("User already exists.", 400);
                    }
                }
                
                string hashedPassword = PasswordHelper.HashPassword(user.password);
                
                var newUser = new
                {
                    Id = userId,
                    firstName = user.firstName,
                    lastName = user.lastName,
                    email = user.email,
                    phone = user.phone,
                    password = hashedPassword 
                };
                
                await client.SetAsync($"Users/{userId}", newUser);
                return new ServerResponse("User registered successfully.", 200);
            }
            else
            {
                return new ServerResponse("Failed to connect to Firebase.", 400);
            }
        }
    }
}