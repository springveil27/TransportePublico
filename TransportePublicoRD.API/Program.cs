using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using TransportePublicoRD.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("Connections");
builder.Services.AddDbContext<DbContextApp>(options =>options.UseSqlServer(connectionString));
builder.Services.AddControllers();
builder.Services.AddScoped<TransportePublicoRD.Infrastructure.Data.Repositories.RouteRepository>();
builder.Services.AddScoped<TransportePublicoRD.Infrastructure.Data.Repositories.ScheduleRepository>();
builder.Services.AddScoped<TransportePublicoRD.Infrastructure.Data.Repositories.StopRepository>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;


});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
