using Sosa.Gym.API;
using Sosa.Gym.Application;
using Sosa.Gym.Common;
using Sosa.Gym.External;
using Sosa.Gym.Persistence;
using Sosa.Gym.Persistence.Seed;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();

builder.Services
    .AddWebApi()
    .AddCommon()
    .AddApplication()
    .AddExternal(builder.Configuration)
    .AddPersistence(builder.Configuration);

// Config CORS 
var allowedOrigins = "AllowedOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowedOrigins, policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:5173",
                "http://127.0.0.1:5173"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});


var app = builder.Build();

await IdentityDataSeed.SeedRolesAsync(app);

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty; 
});

app.UseCors(allowedOrigins);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
