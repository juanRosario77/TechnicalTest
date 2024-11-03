using Microsoft.EntityFrameworkCore;
using TechnicalTest.Core.DTOs;
using TechnicalTest.Core.Interfaces;
using TechnicalTest.Core.Services;
using TechnicalTest.Data;
using TechnicalTest.Data.Interfaces;
using TechnicalTest.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddControllers();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null; // Mantener los nombres de las propiedades tal como están
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddEntityFrameworkSqlServer()
    .AddDbContext<TechnicalTestContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// IOptions
builder.Services.Configure<ValidationConfig>
    (builder.Configuration.GetSection("Validations"));
builder.Services.Configure<AuthenticationConfig>
    (builder.Configuration.GetSection("Authentication"));
// IOptions

// Services
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IPhoneRepository, PhoneRepository>();
builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<IRegistrationService, RegistrationService>();
// Services

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
