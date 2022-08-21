using producer.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<AppConfig>(builder.Configuration.GetSection("AppConfig"));

var app = builder.Build();

app
    .MapGet("/health-check", () => Results.Ok(new { Health = true }))
    .WithName("Health Check");

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();

