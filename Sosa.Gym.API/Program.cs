using Sosa.Gym.API;
using Sosa.Gym.Application;
using Sosa.Gym.Common;
using Sosa.Gym.External;
using Sosa.Gym.Persistence;
using Sosa.Gym.Persistence.Seed;

var builder = WebApplication.CreateBuilder(args);

// Configuración de los servicios del contenedor
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddHttpContextAccessor();

// Inyección de dependencias
builder.Services
    .AddWebApi()
    .AddCommon()
    .AddApplication()
    .AddExternal(builder.Configuration)
    .AddPersistence(builder.Configuration);


var app = builder.Build();

await IdentityDataSeed.SeedRolesAsync(app);

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.UseAuthorization();

app.MapControllers();

app.Run();
