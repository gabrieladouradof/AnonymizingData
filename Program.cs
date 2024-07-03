using Microsoft.EntityFrameworkCore;
using ProtectedDba.Controllers;
using ProtectedDba.Services;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services in the container
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 21))));

// Adiciona o serviço de OpenAiService e injeta a chave da API a partir da configuração
builder.Services.AddSingleton(new OpenAiService(builder.Configuration["OpenAi:ApiKey"]));

// Adiciona o serviço para HashKey
builder.Services.AddSingleton<string>(provider => builder.Configuration["HashKey"]);

builder.Services.AddControllers();
//builder.Services.AddDbContext<AppDbContext>();

var app = builder.Build();

// Configuration of controllers
app.MapControllers();

app.Run();
