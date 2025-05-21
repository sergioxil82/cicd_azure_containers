using Auditory.API.Data.Context;
using Auditory.API.Domain;
using AutoMapper;
using MediatR;
using MongoDB.Driver;
using System.Text.Json;

namespace Auditory.API.Features.UserRecordsHistory.GetUserRecordsHistory
{
    // Query
    public record GetUserRecordsHistoryQuery(string UserName) : IRequest<GetUserRecordsHistoryResult>;

    // Resultado Query
    public record GetUserRecordsHistoryResult(IEnumerable<UserRecordHistory> UserRecordsHistory);

    public class GetUserRecordsHistoryHandler(IMapper mapper, IAuditoryContext context, ILogger<GetUserRecordsHistoryHandler> logger)
        : IRequestHandler<GetUserRecordsHistoryQuery, GetUserRecordsHistoryResult>
    {
        public async Task<GetUserRecordsHistoryResult> Handle(GetUserRecordsHistoryQuery request, CancellationToken cancellationToken)
        {
            // Obtener los registros de usuario por nombre de usuario
            var userRecords = await GetUserRecordsHistory(request.UserName, cancellationToken);
                        
            return new GetUserRecordsHistoryResult(userRecords);
        }

        public async Task<IEnumerable<UserRecordHistory>> GetUserRecordsHistory(string userName, CancellationToken cancellationToken)
        {
            logger.LogInformation("Obteniendo registros horarios del empleado: {userName}", userName);

            // Obtener los registros de usuario por nombre de usuario
            var queryResult = await context.UserRecordCollection.FindAsync(a=>a.UserName == userName, cancellationToken: cancellationToken);

            var result = queryResult.ToListAsync().Result;
            var serializedResults = JsonSerializer.Serialize(result, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });

            logger.LogInformation("Registros horarios obtenidos del empleado: {userName}:\n {userRecords}", userName, serializedResults);

            // Mapear a la respuesta            
            return mapper.Map<IEnumerable<UserRecordHistory>>(result);
        }

    }

    public class GetUserRecordsHistoryProfile : Profile
    {
        public GetUserRecordsHistoryProfile()
        {
            // Mapeo entre modelo y respuesta esperada por el cliente
            CreateMap<UserRecordHistory, UserRecordsHistoryResponse>();
            // Mapeo entre result de la API hacia cliente y respuesta de base de datos
            CreateMap<GetUserRecordsHistoryResult, GetUserRecordsHistoryResponse>();
        }
    }

}
