var builder = WebApplication.CreateBuilder(args);

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowSpecificOrigins",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:5065") // Replace with your app's URL
                .AllowAnyMethod()
                .AllowAnyHeader();
        }
    );
});

// Add the AuthenticationService to the DI container
builder.Services.AddSingleton<AuthenticationService>();

builder.Services.AddControllers(); // If you're using controllers

var app = builder.Build();

// Use the CORS policy
app.UseCors("AllowSpecificOrigins");

// Other middleware
//app.UseAuthorization();

app.MapControllers(); // If you're using controllers

app.Run();
