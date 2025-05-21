using AutoMapper;
using Carter;
using MediatR;

namespace Registry.API.Features.UserWorkTimeRecords.CreateUserWorkTimeRecord
{
    // Request
    public record UserRecordCreateRequest(string UserName, string FirstName, string LastName, DateTime LastRecord, string Mode);
    //Response
    public record UserRecordCreateResponse(bool recordPublished);
    // Endpoint
    public class CreateUserWorkTimeRecordEndpoint(IMapper mapper) : ICarterModule
    {
        // Ruta
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/UserWorkTimeRecord", async (UserRecordCreateRequest request, ISender sender) =>
            {
                // Logica interna
                var command = mapper.Map<UserRecordCreateCommand>(request);
                var result = await sender.Send(command);
                var response = mapper.Map<UserRecordCreateResponse>(result);
                return Results.Ok(result);
            }).WithName("UserWorkTimeRecordCreate") // Nombre Endpoint
            .Produces<UserRecordCreateResponse>(StatusCodes.Status201Created) // Tipo Respuesta HTTP
            .ProducesProblem(StatusCodes.Status400BadRequest) // Tipo Error HTTP
            .WithSummary("Crear nuevo registro horario de empleo")
            .WithDescription("Crear nuevo registro horario de empleo");
        }
    }    
}
