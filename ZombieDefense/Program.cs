using Carter;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection;
using ZombieDefense.Infraestructure.DependencyInjection;
using ZombieDefense.Infraestructure.Middlewares;
using ZombieDefense.Infraestructure.Persistence.Contexts;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
var conectionString = configuration.GetConnectionString("DefaultConnection");
Assembly assemblyApplication = Assembly.Load("ZombieDefense.Application");
string swaggerUrl = "/swagger/v1/swagger.json"!;

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddMediatR(x =>
{
    x.RegisterServicesFromAssemblies(assemblyApplication);
});

builder.Services.AddAutoMapper(x => { }, assemblyApplication);
builder.Services.AddDbContext<ZombieDefenseContext>(x =>
{
    x.UseSqlServer(conectionString);
});

builder.Services.AddServices(configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCarter();
builder.Services.AddSwaggerGen(x =>
{
    x.SwaggerDoc("v1", new() { Title = "Zombie Defense API", Version = "v1", Description = "Api para la estragias defensivas para ataques zombies" });
    x.AddSecurityDefinition("ApiKey", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "Clave API en el encabezado X-API-Key",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Name = "X-API-Key",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Scheme = "ApiKeyScheme"
    });

    x.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            Array.Empty<string>()
        }
    });
});
var app = builder.Build();
app.UseCors("AllowAllOrigins");
app.UseMiddleware<SecurityApiKeyMiddleware>();


app.UseSwagger();
app.UseSwaggerUI(x => x.SwaggerEndpoint(swaggerUrl, "Zombie Defense API"));
app.UseHttpsRedirection();
app.MapCarter();

app.Run();
