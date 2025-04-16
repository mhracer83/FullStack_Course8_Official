namespace SafeVaultAPI.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; } // For securely storing passwords
        public int UserID { get; set; } // Unique identifier
    }
}
