using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using TaskManagementApp.Data;
using TaskManagementApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add DB Context with Azure SQL connection string
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<UnitOfWork>();
builder.Services.AddScoped<IStorageService, StorageService>();

// Add Controllers and Swagger
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    }); builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());
});

var app = builder.Build();

// Enable Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAllOrigins"); // Enable CORS policy

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
