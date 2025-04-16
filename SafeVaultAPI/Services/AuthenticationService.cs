using MySql.Data.MySqlClient;
using SafeVault.Shared; // For InputSanitizer

public class AuthenticationService
{
    private readonly string _connectionString;

    public AuthenticationService(string connectionString)
    {
        _connectionString = connectionString;
    }

    // Register a new user with hashed password
    public bool RegisterUser(string username, string email, string password, string role)
    {
        // Sanitize inputs to prevent SQL injection
        username = InputSanitizer.SanitizeInput(username);

        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();

            // Check if username exists
            var checkCommand = new MySqlCommand(
                "SELECT COUNT(*) FROM Users WHERE Username = @Username",
                connection
            );
            checkCommand.Parameters.AddWithValue("@Username", username);

            var count = Convert.ToInt32(checkCommand.ExecuteScalar());
            if (count > 0)
            {
                return false; // Username already exists
            }

            // Hash the password and insert the user with a role
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            var insertCommand = new MySqlCommand(
                "INSERT INTO Users (Username, Email, HashedPassword, Role) VALUES (@Username, @Email, @HashedPassword, @Role)",
                connection
            );
            insertCommand.Parameters.AddWithValue("@Username", username);
            insertCommand.Parameters.AddWithValue("@Email", email);
            insertCommand.Parameters.AddWithValue("@HashedPassword", hashedPassword);
            insertCommand.Parameters.AddWithValue("@Role", role);

            insertCommand.ExecuteNonQuery();
            return true; // User registered successfully
        }
    }

    // Authenticate a user by verifying the password
    public bool AuthenticateUser(string username, string password)
    {
        // Sanitize inputs to prevent SQL injection
        username = InputSanitizer.SanitizeInput(username);

        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();

            // Fetch the user record
            var command = new MySqlCommand(
                "SELECT HashedPassword FROM Users WHERE Username = @Username",
                connection
            );
            command.Parameters.AddWithValue("@Username", username);

            var hashedPassword = command.ExecuteScalar() as string;
            if (hashedPassword == null)
            {
                return false; // User not found
            }

            return BCrypt.Net.BCrypt.Verify(password, hashedPassword); // Verify hashed password
        }
    }

    public string GetUserRole(string username)
    {
        // Sanitize inputs to prevent SQL injection
        username = InputSanitizer.SanitizeInput(username);

        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            var command = new MySqlCommand(
                "SELECT Role FROM Users WHERE Username = @Username",
                connection
            );
            command.Parameters.AddWithValue("@Username", username);

            var role = command.ExecuteScalar() as string;
            return role ?? "Unknown"; // Default to "Unknown" if no role is found
        }
    }
}
