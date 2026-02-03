using Microsoft.EntityFrameworkCore;
using Sosa.Gym.API;
using Sosa.Gym.Application;
using Sosa.Gym.Application.Exceptions;
using Sosa.Gym.Common;
using Sosa.Gym.External;
using Sosa.Gym.Persistence;
using Sosa.Gym.Persistence.DataBase;
using Sosa.Gym.Persistence.Seed;

var builder = WebApplication.CreateBuilder(args);

// Controllers + filtro global
builder.Services.AddControllers(options => options.Filters.Add<ExceptionManager>());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();

builder.Services
    .AddWebApi()
    .AddCommon()
    .AddApplication()
    .AddExternal(builder.Configuration)
    .AddPersistence(builder.Configuration);

// CORS
var allowedOrigins = "AllowedOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowedOrigins, policy =>
    {
        policy
            .WithOrigins("http://localhost:5173", "http://127.0.0.1:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DataBaseService>();
    await db.Database.MigrateAsync();
}

// Seed roles/admin
await IdentityDataSeed.SeedRolesAsync(app);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(allowedOrigins);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
