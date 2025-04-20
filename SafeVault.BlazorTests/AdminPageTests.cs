using Bunit;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using SafeVault.Pages;
using Xunit;

public class AdminPageTests : TestContext
{
    [Fact]
    public void AdminPage_ShouldRender_WhenUserIsAdmin()
    {
        Services.AddScoped<AuthenticationStateProvider>(_ => new TestAuthStateProvider("Admin"));
        Services.AddAuthorizationCore();

        var cut = RenderComponent<AdminPage>();

        cut.Markup.Contains("You are an admin!");
    }

    [Fact]
    public void AdminPage_ShouldNotRender_WhenUserIsNotAdmin()
    {
        Services.AddScoped<AuthenticationStateProvider>(_ => new TestAuthStateProvider("User"));
        Services.AddAuthorizationCore();

        var cut = RenderComponent<AdminPage>();

        Assert.DoesNotContain("You are an admin!", cut.Markup);
    }
}
