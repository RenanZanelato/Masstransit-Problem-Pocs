using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Playground.Consumer;
using Playground.Messages;

namespace Playground.Infra
{
    public static class ServiceBusExtensions
    {
        public static IServiceCollection AddMassTransitServiceBus(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IConsumer<SampleCreatedEvent>, SampleCreatedConsumer>()
                .AddScoped<IConsumer<SampleDeletedEvent>, SampleDeletedConsumer>()
                .AddScoped<IConsumer<SampleUpdatedEvent>, SampleUpdatedConsumer>()
                .AddMassTransit(busConfigure =>
                {
                    busConfigure.AddConsumer<IConsumer<SampleCreatedEvent>>();
                    busConfigure.AddConsumer<IConsumer<SampleUpdatedEvent>>();
                    busConfigure.AddConsumer<IConsumer<SampleDeletedEvent>>();

                    busConfigure.UsingAzureServiceBus((context, cfg) =>
                    {
                        //get connection on the file appsettings.json
                        var configuration = context.GetRequiredService<IConfiguration>();
                        cfg.Host(configuration.GetConnectionString("AzureServiceBus"));

                        cfg.ConfigureEndpoints(context);

                        //sbt-example-sample-SampleCreatedEvent
                        cfg.ConfigureTopologyEntityName<SampleCreatedEvent>("example", "sample");
                        //sbt-example-sample-SampleUpdatedEvent
                        cfg.ConfigureTopologyEntityName<SampleUpdatedEvent>("example", "sample");
                        //sbt-example-sample-SampleDeletedEvent
                        cfg.ConfigureTopologyEntityName<SampleDeletedEvent>("example", "sample");
                        
                        // configuring subscriptions
                        cfg.ConfigureSubscription<SampleCreatedEvent>(context);
                        cfg.ConfigureSubscription<SampleUpdatedEvent>(context);
                        cfg.ConfigureSubscription<SampleDeletedEvent>(context);
                    });
                });

            return serviceCollection;
        }

        //Configure Topic Name to sbt-{environmentName}-{context}-{TMessage}
        private static void ConfigureTopologyEntityName<TMessage>(this IServiceBusBusFactoryConfigurator cfg,
            string environmentName, string context)
            where TMessage : class
            => cfg.Message<TMessage>(cfgTopology =>
            {
                var entityName = MessageEntityNameFormatter<TMessage>.Create(environmentName, context);
                cfgTopology.SetEntityNameFormatter(entityName);
            });

        private static void ConfigureSubscription<TMessage>(this IServiceBusBusFactoryConfigurator cfg, IBusRegistrationContext context)
            where TMessage : class
            => cfg.SubscriptionEndpoint<TMessage>("sbts-example",
                endpointConfig => { endpointConfig.ConfigureConsumer<IConsumer<TMessage>>(context); });
    }

    public class MessageEntityNameFormatter<TMessage> : IMessageEntityNameFormatter<TMessage>
        where TMessage : class
    {
        private readonly string _environmentName;
        private readonly string _context;

        protected MessageEntityNameFormatter(string environmentName, string context)
        {
            _environmentName = environmentName;
            _context = context;
        }

        public static MessageEntityNameFormatter<TMessage> Create(string environmentName, string context)
            => new MessageEntityNameFormatter<TMessage>(environmentName, context);

        public string FormatEntityName()
        {
            return $"sbt-{_environmentName}-{_context}-{typeof(TMessage).Name}";
        }
    }
}