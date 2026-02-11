using Microsoft.EntityFrameworkCore;
using TasksService.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TasksDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient("Gateway", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["GatewayBaseUrl"]!);
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
