var builder = WebApplication.CreateBuilder(args);

// Register AWS Secrets Manager Service
builder.Services.AddSingleton<SecretsManagerService>();

// Add Controllers
builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
