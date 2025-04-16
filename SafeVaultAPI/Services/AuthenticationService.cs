using SafeVaultAPI.Models;

public class AuthenticationService
{
    private readonly List<User> _users = new List<User>();

    // Register a new user with hashed password
    public bool RegisterUser(string username, string password)
    {
        if (_users.Any(u => u.Username == username))
        {
            return false; // Username already exists
        }

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
        _users.Add(new User { Username = username, HashedPassword = hashedPassword });
        return true; // User registered successfully
    }

    // Authenticate a user by verifying the password
    public bool AuthenticateUser(string username, string password)
    {
        var user = _users.FirstOrDefault(u => u.Username == username);
        if (user == null)
        {
            return false; // User not found
        }

        return BCrypt.Net.BCrypt.Verify(password, user.HashedPassword); // Verify hashed password
    }
}
