using Components.Communication.MessageBroker.Configuration;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Components.Communication.MessageBroker
{
    public static class ExtendServices
    {
        // Metodo Extension
        public static IServiceCollection AddMessageBroker(this IServiceCollection services, IConfiguration configuration, Assembly? assembly=null)
        {
            // Mapeado Configuración RabbitMQ
            var rabbitMqOptions = new RabbitMqHostOptions();
            configuration.GetSection("MessageBroker").Bind(rabbitMqOptions);

            // Configuración Mass Transit
            services.AddMassTransit(busConfigurarion =>
            {
                // Kebab-case
                busConfigurarion.SetKebabCaseEndpointNameFormatter();

                // Registrar Consumidores
                if (assembly != null)
                {
                    busConfigurarion.AddConsumers(assembly);
                }

                // Configurar RabbitMQ
                busConfigurarion.UsingRabbitMq((context, rabbitMqConfiguration) =>
                {
                    rabbitMqConfiguration.Host(new Uri(rabbitMqOptions.Host), host =>
                    {
                        // Host, User, Password
                        host.Username(rabbitMqOptions.UserName);
                        host.Password(rabbitMqOptions.Password);
                    });
                    // Configurar colas
                    rabbitMqConfiguration.ConfigureEndpoints(context);
                });
            });
            return services;
        }
    }
}
