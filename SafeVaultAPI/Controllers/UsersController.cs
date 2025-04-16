using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace SafeVaultAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public UsersController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetUser(string username)
        {
            try
            {
                // Get connection string from appsettings.json
                var connectionString = _configuration.GetConnectionString("MySqlConnection");

                // Connect to the database
                using (var connection = new MySqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    // Parameterized query to prevent SQL injection
                    string query = "SELECT Username, Email FROM Users WHERE Username = @Username";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                while (await reader.ReadAsync())
                                {
                                    var user = new
                                    {
                                        Username = reader["Username"].ToString(),
                                        Email = reader["Email"].ToString(),
                                    };
                                    return Ok(user);
                                }
                            }
                            else
                            {
                                return NotFound($"User with username '{username}' not found.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle errors
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            // Fallback in case of unexpected flow
            return BadRequest("An unexpected error occurred.");
        }
    }
}
