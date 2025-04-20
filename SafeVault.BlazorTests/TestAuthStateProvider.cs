using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

public class TestAuthStateProvider : AuthenticationStateProvider
{
    private readonly ClaimsPrincipal _user;

    public TestAuthStateProvider(string role)
    {
        _user = new ClaimsPrincipal(
            new ClaimsIdentity(
                new[] { new Claim(ClaimTypes.Name, "TestUser"), new Claim(ClaimTypes.Role, role) },
                "mock"
            )
        );
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync() =>
        Task.FromResult(new AuthenticationState(_user));
}
