namespace SafeVaultAPI.Models
{
    public class LoginRequestDTO
    {
        public string Username { get; set; }
        public string Password { get; set; } // Raw password sent by the frontend
    }
}
