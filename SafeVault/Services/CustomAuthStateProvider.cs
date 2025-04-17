using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace SafeVault.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private ClaimsPrincipal _user = new ClaimsPrincipal(new ClaimsIdentity());

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return Task.FromResult(new AuthenticationState(_user));
        }

        public Task MarkUserAsAuthenticated(string username, string role)
        {
            var identity = new ClaimsIdentity(
                new[] { new Claim(ClaimTypes.Name, username), new Claim(ClaimTypes.Role, role) },
                "apiauth_type"
            );

            _user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_user)));
            return Task.CompletedTask;
        }

        public Task MarkUserAsLoggedOut()
        {
            _user = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_user)));
            return Task.CompletedTask;
        }
    }
}
