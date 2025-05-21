using AutoMapper;
using Carter;
using MediatR;

namespace Auditory.API.Features.UserRecordsHistory.GetUserRecordsHistory
{
    // Response
    public record GetUserRecordsHistoryResponse(IEnumerable<UserRecordsHistoryResponse> UserRecordsHistory);

    // Endpoint
    public class GetUserRecordsHistoryEndpoint(IMapper mapper, ILogger<GetUserRecordsHistoryEndpoint> logger) : ICarterModule
    {
        // Ruta
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            // Request
            app.MapGet("/UserRecordsHistory/{userName}", async (string userName, ISender sender) =>
            {
                // Lógica Interna
                logger.LogInformation("Obtener registors horarios del empleado: {userName}", userName);

                var query = new GetUserRecordsHistoryQuery(userName);
                var result = await sender.Send(query);

                logger.LogInformation("Obtenidos {userRecordsCount} registros horarios del empleado: {userName}", result.UserRecordsHistory.Count(), userName);

                var response = mapper.Map<GetUserRecordsHistoryResponse>(result);
                return Results.Ok(response.UserRecordsHistory);
            })
            .WithName("GetUserRecordsHistoryByUserName") // Nombre del endpoint
            .Produces<GetUserRecordsHistoryResponse>(StatusCodes.Status200OK) // Tipo Respuesta HTTP
            .ProducesProblem(StatusCodes.Status400BadRequest) // Tipo Respuesta Error
            .WithSummary("Registro de horarios de un empleado") // Título del endpoint
            .WithDescription("Obtiene los registros horarios de un empleado por su nombre de usuario"); // Descripción del endpoint
        }
    }
}
