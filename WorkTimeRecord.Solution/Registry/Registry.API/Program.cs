using Carter;
using Components.Communication.MessageBroker;
using Microsoft.EntityFrameworkCore;
using Registry.API.Data.Context;

var builder = WebApplication.CreateBuilder(args);
// AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Documentación Swagger MinimalAPI
builder.Services.AddEndpointsApiExplorer();

// Generador de documentación Swagger
builder.Services.AddSwaggerGen(swagger =>
{
    swagger.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo() 
    { 
        Title = "Registry API", 
        Description = "API para el último registro de horario de cada empleado",
        Version = "v1" });    
});

// Registrar Carter
builder.Services.AddCarter();
// MediatR
builder.Services.AddMediatR(configuration => { configuration.RegisterServicesFromAssembly(typeof(Program).Assembly); });
// Registrar Servicio
builder.Services.AddScoped(typeof(IRegistryContext), typeof(RegistryPostgresContext));

// Broker
builder.Services.AddMessageBroker(builder.Configuration);

// Configurar contexto de base de datos
builder.Services.AddDbContext<RegistryPostgresContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("UserRecord"));
});

// Configura CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy
            .WithOrigins("http://localhost:5100")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

// Habilita Swagger
app.UseSwagger();
// Habilita la UI de Swagger
app.UseSwaggerUI(c =>c.SwaggerEndpoint("/swagger/v1/swagger.json", "Registry API v1"));

// Usa la política de CORS
app.UseCors("AllowFrontend");

// Buscar rutas
app.MapCarter();

app.Run();
