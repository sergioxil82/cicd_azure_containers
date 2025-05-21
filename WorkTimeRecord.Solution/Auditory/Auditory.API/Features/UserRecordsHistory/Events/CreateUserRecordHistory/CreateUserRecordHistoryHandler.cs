using Auditory.API.Data;
using Auditory.API.Data.Context;
using AutoMapper;
using Components.Communication.Events;
using MassTransit;
using MediatR;
using System.Text.Json;
using Auditory.API.Domain;

namespace Auditory.API.Features.UserRecordsHistory.Events.CreateUserRecordHistory
{
    // Comando
    public record CreateUserRecordHistoryCreatedCommand(UserWorkTimeRecordCreatedEventDto UserWorkTimeRecordCreatedEventDto) : IRequest<CreateUserRecordHistoryCreatedResult>;
    //Resultado Comando
    public record CreateUserRecordHistoryCreatedResult(string Id);

    // Consumidor de eventos
    public class CreateUserRecordHistoryHandler(ISender sender, IMapper mapper, ILogger<CreateUserRecordHistoryHandler> logger)
        : IConsumer<UserWorkTimeRecordCreatedEvent> // Interfaz Consumidor
    {
        // Metodo Consumir
        public async Task Consume(ConsumeContext<UserWorkTimeRecordCreatedEvent> context)
        {
            // Evento Recibido
            var eventMessage = context.Message;
            logger.LogInformation("Consumiendo evento 'CreateUserRecordHistoryCreatedEvent':\n EventId: {EventId}\n     " +
                "Event Type: {EventType}", eventMessage.Id, eventMessage.EventType);
            // Preparamos el comando
            var command = new CreateUserRecordHistoryCreatedCommand(mapper.Map<UserWorkTimeRecordCreatedEventDto>(eventMessage));
            logger.LogInformation("Enviado comando CreateUserRecordHistoryCreatedCommand con el contenido " +
                "del mensaje:\n     MessageContent: {MessageContent}", eventMessage);

            // Emitir el comando
            await sender.Send(command);
        }

    }
    // Manejador del Comando
    public class CreateUserRecordHistoryCreatedCommandHandler(IMapper mapper, IAuditoryContext context, ILogger<CreateUserRecordHistoryCreatedCommandHandler> logger)
    : IRequestHandler<CreateUserRecordHistoryCreatedCommand, CreateUserRecordHistoryCreatedResult>
    {
        // Método Manejador
        public async Task<CreateUserRecordHistoryCreatedResult> Handle(CreateUserRecordHistoryCreatedCommand request, CancellationToken cancellationToken)
        {
            // Serializamos el DTO
            var userWorkTimeRecordCreatedEventDto = request.UserWorkTimeRecordCreatedEventDto;
            var serializedDto = JsonSerializer.Serialize(userWorkTimeRecordCreatedEventDto, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
            logger.LogInformation("Se va a persistir en Mongo 'UserRecord':\n     UserRecord: {UserRecord}", serializedDto);

            // Conversor de DTO a Entidad MongoDB
            var userRecord = mapper.Map<UserRecordHistory>(request.UserWorkTimeRecordCreatedEventDto);
            var userRecordMongo = mapper.Map<UserRecordHistoryMongo>(userRecord);

            // Persistimos en MongoDB
            var id = await CreateUserRecordHistoryRecord(userRecordMongo, cancellationToken);

            logger.LogInformation("Entidad UserRecordHistory persistida en MongoDB con Id: {Id}", id);

            return new CreateUserRecordHistoryCreatedResult(id);
        }

        // Persistencia MongoDB
        public async Task<string> CreateUserRecordHistoryRecord(UserRecordHistoryMongo userRecord, CancellationToken cancellationToken)
        {
            // Insertamos en la colección de MongoDB un registro de trabajo
            await context.UserRecordCollection.InsertOneAsync(userRecord, cancellationToken: cancellationToken);
            return userRecord.Id;
        }
    }

    // Profile de AutoMapper
    public class CreateUserRecordHistoryProfile : Profile
    {
        public CreateUserRecordHistoryProfile()
        {
            // Mapeo de DTO a Modelo del dominio
            CreateMap<UserRecordHistory, UserWorkTimeRecordCreatedEventDto>().ReverseMap();

            // Mapeo de Evento a DTO
            CreateMap<UserWorkTimeRecordCreatedEvent, UserWorkTimeRecordCreatedEventDto>();

        }
    }
}
