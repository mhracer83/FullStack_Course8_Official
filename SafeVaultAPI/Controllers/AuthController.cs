using Microsoft.AspNetCore.Mvc;
using SafeVaultAPI.Models; // For the User class

//using SafeVaultAPI.Services; // For the AuthenticationService

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthenticationService _authenticationService;

    public AuthController(AuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] User user)
    {
        var success = _authenticationService.RegisterUser(user.Username, user.HashedPassword);
        if (!success)
        {
            return BadRequest("Username already exists.");
        }
        return Ok("User registered successfully.");
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] User user)
    {
        var isAuthenticated = _authenticationService.AuthenticateUser(
            user.Username,
            user.HashedPassword
        );
        if (!isAuthenticated)
        {
            return Unauthorized("Invalid username or password.");
        }
        return Ok("Login successful.");
    }
}
