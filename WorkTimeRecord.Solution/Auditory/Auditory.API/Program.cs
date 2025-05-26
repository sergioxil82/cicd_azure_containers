using Auditory.API.Data.Context;
using Carter;
using Components.Communication.MessageBroker;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Documentaci�n Swagger MinimalAPI
builder.Services.AddEndpointsApiExplorer();
// Generador de Documentaci�n Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo() { Title = "Auditory API", Version = "v1", Description="API de Auditor�a de registros de horario de los empleados" });    
});

// MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly));
// Contexto
builder.Services.AddScoped(typeof(IAuditoryContext), typeof(AuditoryMongoContext));

// Broker
builder.Services.AddMessageBroker(builder.Configuration, Assembly.GetExecutingAssembly());
// Registrar Carter
builder.Services.AddCarter();

// Configura CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

app.UseSwagger(); // Habilitar Swagger
// Habilita UI de Swagger
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Auditory API V1");   
});

// Usa la pol�tica de CORS
app.UseCors("AllowAll");

// Buscar Rutas de Carter
app.MapCarter(); // Busca las rutas de definidas pos las implementaciones de ICarterModule

app.Run();
