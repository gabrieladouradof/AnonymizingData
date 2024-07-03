using Microsoft.EntityFrameworkCore;
using ProtectedDba.Controllers;
using ProtectedDba.Services;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

//Adds database context with configuration file connection string
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 21))));

//builder.Services.AddSingleton(new OpenAiService(builder.Configuration["OpenAi:ApiKey"]));

// Add OpenAiService service and inject API key from configuration
builder.Services.AddSingleton(sp => 
{
    var apiKey = builder.Configuration["OpenAi:ApiKey"] 
        ?? throw new ArgumentNullException("OpenAi:ApiKey", "OpenAI API Key is missing.");
    return new OpenAiService(apiKey);
});


//Add service for HashKey
builder.Services.AddSingleton(sp => 
{
    var hashKey = builder.Configuration["HashKey"] 
        ?? throw new ArgumentNullException("HashKey", "HashKey configuration value is missing.");
    return hashKey;
});

//builder.Services.AddSingleton<string>(provider => builder.Configuration["HashKey"]);

builder.Services.AddControllers();
var app = builder.Build();
app.MapControllers();
app.Run();

