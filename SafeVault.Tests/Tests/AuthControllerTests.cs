using Microsoft.AspNetCore.Mvc;
using Moq;
using SafeVaultAPI.Controllers;
using SafeVaultAPI.Models;
using Xunit;

public class AuthControllerTests
{
    [Fact]
    public void Login_ReturnsOk_WhenCredentialsAreValid()
    {
        // Arrange
        var mockAuthService = new Mock<IAuthenticationService>();
        mockAuthService.Setup(s => s.AuthenticateUser("admin", "pass")).Returns(true);
        mockAuthService.Setup(s => s.GetUserRole("admin")).Returns("Admin");

        var controller = new AuthController(mockAuthService.Object);

        var loginRequest = new LoginRequestDTO { Username = "admin", Password = "pass" };

        // Act
        var result = controller.Login(loginRequest);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
    }
}
