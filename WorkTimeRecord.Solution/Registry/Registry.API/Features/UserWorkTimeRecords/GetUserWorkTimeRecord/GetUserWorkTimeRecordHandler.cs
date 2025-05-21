using MediatR;
using Registry.API.Data.Context;
using Registry.API.Domain;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Registry.API.Features.UserWorkTimeRecords.GetUserWorkTimeRecord
{
    public record GetUserWorkTimeRecordQuery(string UserName) : IRequest<GetUserWorkTimeRecordResult>;
    public record GetUserWorkTimeRecordResult(UserWorkTimeRecord UserWorkTimeRecord);
    public class GetUserWorkTimeRecordHandler(IRegistryContext context, ILogger<GetUserWorkTimeRecordHandler> logger) : IRequestHandler<GetUserWorkTimeRecordQuery, GetUserWorkTimeRecordResult>
    {
        public async Task<GetUserWorkTimeRecordResult> Handle(GetUserWorkTimeRecordQuery request, CancellationToken cancellationToken)
        {
            var userWorkTimeRecord = await GetUserWorkTimeRecord(request.UserName, cancellationToken);
            return new GetUserWorkTimeRecordResult(userWorkTimeRecord);
        }

        public async Task<UserWorkTimeRecord> GetUserWorkTimeRecord(string userName, CancellationToken cancellationToken)
        {
            logger.LogInformation("Buscar en Postgresql último registro horario del empleado: {UserName}", userName);

            var userRecord = await context.UserWorkTimeRecords.FirstOrDefaultAsync(x => x.UserName == userName, cancellationToken);
            var serializedResult = JsonSerializer.Serialize(userRecord, 
                new JsonSerializerOptions { WriteIndented = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping 
            });

            if(userRecord == null)
            {
                logger.LogWarning("No se encontró el registro horario del empleado: {UserName}", userName);
                userRecord = new UserWorkTimeRecord();
            }
            else
                logger.LogInformation("Registro horario encontrado: {UserName} \n {serializedResult}", userRecord.UserName, serializedResult);

            return userRecord;
        }

        public class GetUserWorkTimeRecordProfile : Profile
        {
            public GetUserWorkTimeRecordProfile()
            {
                // Mapeo entre modelo y respuesta esperada por el cliente
                CreateMap<UserWorkTimeRecord, UserWorkTimeRecordResponse>();
                // Mapeo entre result de la API hacia cliente y respuesta de base de datos
                CreateMap<GetUserWorkTimeRecordResult, GetUserWorkTimeRecordResponse>();
            }
        }
    }
}
