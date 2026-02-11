using Microsoft.EntityFrameworkCore;
using TasksService.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TasksDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHealthChecks();

var usersServiceBaseUrl = builder.Configuration["UsersServiceBaseUrl"]
    ?? throw new InvalidOperationException("UsersServiceBaseUrl is not configured.");
var projectsServiceBaseUrl = builder.Configuration["ProjectsServiceBaseUrl"]
    ?? throw new InvalidOperationException("ProjectsServiceBaseUrl is not configured.");

builder.Services.AddHttpClient("UsersService", client =>
{
    client.BaseAddress = new Uri(usersServiceBaseUrl);
});

builder.Services.AddHttpClient("ProjectsService", client =>
{
    client.BaseAddress = new Uri(projectsServiceBaseUrl);
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
ApplyMigrationsWithRetry(app);

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();

static void ApplyMigrationsWithRetry(WebApplication app)
{
    const int maxRetries = 10;
    var delay = TimeSpan.FromSeconds(3);

    for (var attempt = 1; attempt <= maxRetries; attempt++)
    {
        try
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TasksDbContext>();
            dbContext.Database.Migrate();
            return;
        }
        catch when (attempt < maxRetries)
        {
            Thread.Sleep(delay);
        }
    }

    using var finalScope = app.Services.CreateScope();
    finalScope.ServiceProvider.GetRequiredService<TasksDbContext>().Database.Migrate();
}
