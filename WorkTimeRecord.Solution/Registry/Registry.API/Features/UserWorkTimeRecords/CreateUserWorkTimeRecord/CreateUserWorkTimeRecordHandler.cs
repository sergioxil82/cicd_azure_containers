using AutoMapper;
using Components.Communication.Events;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Registry.API.Data.Context;
using Registry.API.Domain;

namespace Registry.API.Features.UserWorkTimeRecords.CreateUserWorkTimeRecord
{
    // Command
    public record UserRecordCreateCommand(string UserName, string FirstName, string LastName, DateTime LastRecord, string Mode) : IRequest<UserRecordCreateResult>;
    // Result
    public record UserRecordCreateResult(bool recordPublished);
    // Manejador del Comando
    public class CreateUserWorkTimeRecordHandler(IMapper mapper, IRegistryContext context, IPublishEndpoint publishEndpoint, ILogger<CreateUserWorkTimeRecordHandler> logger) : IRequestHandler<UserRecordCreateCommand, UserRecordCreateResult>
    {
        // Método manejador
        public async Task<UserRecordCreateResult> Handle(UserRecordCreateCommand request, CancellationToken cancellationToken)
        {
            // Crear Registro
            var userRecordEntity = await CreateUserRecord(mapper.Map<UserWorkTimeRecord>(request), cancellationToken);
            // Publicar evento
            await PublishCreatedUserRecordEvent(mapper.Map<UserWorkTimeRecordCreatedEvent>(userRecordEntity), cancellationToken);

            return new UserRecordCreateResult(true);
        }
        // Acceso a datos
        public async Task<UserWorkTimeRecord> CreateUserRecord(UserWorkTimeRecord userRecord, CancellationToken cancellationToken)
        {
          var existingRecord = await context.UserWorkTimeRecords.FirstOrDefaultAsync(x => x.UserName == userRecord.UserName, cancellationToken);
            if (existingRecord == null) context.UserWorkTimeRecords.Add(userRecord);
            else
            {
                existingRecord.LastRecord = userRecord.LastRecord;
                existingRecord.Mode = userRecord.Mode;             
            }
            await context.SaveChangesAsync();

            logger.LogInformation("Registro creado en PostgreSQL: {UserName}", userRecord.UserName);

            return userRecord;
        }

        // Publicar evento
        public async Task<bool> PublishCreatedUserRecordEvent(UserWorkTimeRecordCreatedEvent userWorkTimeRecordCreatedEvent, CancellationToken cancellationToken)
        {
            await publishEndpoint.Publish(userWorkTimeRecordCreatedEvent, cancellationToken);
            return true;
        }
    }

    // AutoMapper
    public class CreateUserWorkTimeRecordProfile : Profile
    {
        public CreateUserWorkTimeRecordProfile()
        {
            // Mapear de Command a entitdad del dominio
            CreateMap<UserRecordCreateCommand, UserWorkTimeRecord>();
            // Mapear de request a command
            CreateMap<UserRecordCreateRequest, UserRecordCreateCommand>().ReverseMap();
            // Mapear de entidad del dominio a evento
            CreateMap<UserWorkTimeRecord, UserWorkTimeRecordCreatedEvent>();
            // Mapear entre result de la API hacua cliente y response de base de datos
            CreateMap<UserRecordCreateResult, UserRecordCreateResponse>().ReverseMap();
        }
    }
}
