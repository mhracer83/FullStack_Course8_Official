public interface IAuthenticationService
{
    bool AuthenticateUser(string username, string password);
    bool RegisterUser(string username, string email, string password, string role);
    string GetUserRole(string username);
}
