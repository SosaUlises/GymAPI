using Sosa.Gym.Application;
using Sosa.Gym.Common;
using Sosa.Gym.External;
using Sosa.Gym.Persistence;
using Sosa.Gym.Persistence.Seed;

var builder = WebApplication.CreateBuilder(args);

// Configuración de los servicios del contenedor
builder.Services.AddControllers();
builder.Services.AddOpenApi();


// Inyección de dependencias
builder.Services
    .AddCommon()
    .AddApplication()
    .AddExternal(builder.Configuration)
    .AddPersistence(builder.Configuration);


var app = builder.Build();

await IdentityDataSeed.SeedRolesAsync(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
