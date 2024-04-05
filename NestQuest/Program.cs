using Microsoft.EntityFrameworkCore;
using NestQuest.Data;
using NestQuest.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ISignUpServices, SignUpServices>();
builder.Services.AddScoped<ILogInService, LogInService>();

builder.Services.AddMemoryCache();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Setup DB connection
var connectionString = builder.Configuration.GetConnectionString("AppDbConnectString");
builder.Services.AddDbContext<DBContext>(options =>
options.UseMySql(connectionString,
    ServerVersion.AutoDetect(connectionString)));

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