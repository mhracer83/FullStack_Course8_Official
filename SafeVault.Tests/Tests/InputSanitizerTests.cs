using SafeVault.Shared;
using Xunit;

namespace SafeVault.Tests
{
    public class InputSanitizerTests
    {
        [Theory]
        [InlineData("<script>", "script")]
        [InlineData("hello;world", "helloworld")]
        [InlineData("user'name", "username")]
        [InlineData("cleanInput", "cleanInput")]
        public void SanitizeInput_RemovesMaliciousCharacters(string input, string expected)
        {
            var result = InputSanitizer.SanitizeInput(input);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void SanitizeInput_NullInput_ReturnsNull()
        {
            var result = InputSanitizer.SanitizeInput(null);
            Assert.Null(result);
        }

        [Theory]
        [InlineData("' OR 1=1 --", " OR 1=1 ")] // Basic SQL Injection
        [InlineData("'; DROP TABLE Users; --", " DROP TABLE Users ")] // Dangerous SQL Query
        [InlineData("admin' --", "admin ")] // Commented SQL Injection
        [InlineData(
            "SELECT * FROM Users WHERE username = 'admin' AND password = 'password'",
            "SELECT * FROM Users WHERE username = admin AND password = password"
        )] // Complex injection
        public void SanitizeInput_SQLInjection_RemovesMaliciousSQLCharacters(
            string input,
            string expected
        )
        {
            // Act: Sanitize the input
            var result = InputSanitizer.SanitizeInput(input);

            // Assert: Ensure the result matches expected sanitized output
            Assert.Equal(expected, result);
        }

        // Ensure valid inputs remain unchanged
        [Theory]
        [InlineData("validInput", "validInput")]
        [InlineData("1234", "1234")]
        [InlineData("hello world", "hello world")]
        public void SanitizeInput_ValidInput_ReturnsSame(string input, string expected)
        {
            var result = InputSanitizer.SanitizeInput(input);
            Assert.Equal(expected, result);
        }
    }
}
