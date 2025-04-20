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
/*builder.Services.AddScoped(sp => new AuthenticationService(
    builder.Configuration.GetConnectionString("MySqlConnection") // Pass the connection string
));*/

builder.Services.AddScoped<IAuthenticationService>(sp => new AuthenticationService(
    builder.Configuration.GetConnectionString("MySqlConnection")
));

builder.Services.AddControllers(); // If you're using controllers

var app = builder.Build();

// Use the CORS policy
app.UseCors("AllowSpecificOrigins");

// Other middleware
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers(); // If you're using controllers

app.Run();
