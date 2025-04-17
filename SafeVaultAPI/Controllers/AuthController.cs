using Microsoft.AspNetCore.Mvc;
using SafeVault.Shared; // For InputSanitizer
using SafeVaultAPI.Models; // For the User class

[ApiController]
[Route("api/auth")]
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
        if (
            user == null
            || string.IsNullOrEmpty(user.Username)
            || string.IsNullOrEmpty(user.HashedPassword)
        )
        {
            return BadRequest("Invalid user data.");
        }

        // Assign a default role (e.g., "User") on the backend
        var defaultRole = "User";

        // Register the user
        var success = _authenticationService.RegisterUser(
            user.Username,
            user.Email,
            user.HashedPassword,
            defaultRole
        );
        if (!success)
        {
            return Conflict("Username already exists.");
        }

        return Ok("User registered successfully.");
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequestDTO loginRequest)
    {
        if (
            loginRequest == null
            || string.IsNullOrEmpty(loginRequest.Username)
            || string.IsNullOrEmpty(loginRequest.Password)
        )
        {
            return BadRequest("Invalid username or password.");
        }

        var isAuthenticated = _authenticationService.AuthenticateUser(
            loginRequest.Username,
            loginRequest.Password
        );
        if (!isAuthenticated)
        {
            return Unauthorized("Invalid username or password.");
        }

        var role = _authenticationService.GetUserRole(loginRequest.Username);
        return Ok(new { Role = role });
    }
}
