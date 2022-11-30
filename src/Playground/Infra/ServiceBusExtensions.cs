using System;
using Azure.Storage.Blobs;
using MassTransit;
using MassTransit.MessageData;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Playground.Consumer;
using Playground.Messages;

namespace Playground.Infra
{
    public static class ServiceBusExtensions
    {
        public static IServiceCollection AddMassTransitServiceBus(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IMessageDataRepository>(container =>
            {
                //return new InMemoryMessageDataRepository();
                var configuration = container.GetRequiredService<IConfiguration>();
                var client = new BlobServiceClient(configuration.GetConnectionString("Storage"));
                return client.CreateMessageDataRepository(containerName: "servicebus-heavy-messages");
            });

            serviceCollection.AddScoped<IConsumer<SampleItensMessageDataEvent>, SampleItensCreatedConsumer>();

            serviceCollection
                .AddMassTransit(busConfigure =>
                {
                    MessageDataDefaults.AlwaysWriteToRepository = false;
                    MessageDataDefaults.TimeToLive = TimeSpan.FromDays(7); 
                    MessageDataDefaults.Threshold = 1;

                    busConfigure.AddConsumer<IConsumer<SampleItensMessageDataEvent>>();
                    busConfigure.UsingAzureServiceBus((context, serviceBusConfigure) =>
                    {
                        var configuration = context.GetRequiredService<IConfiguration>();
                        serviceBusConfigure.Host(configuration.GetConnectionString("AzureServiceBus"));

                        serviceBusConfigure.UseMessageData(context.GetRequiredService<IMessageDataRepository>());

                        serviceBusConfigure.ConfigureEndpoints(context);

                        serviceBusConfigure.Message<SampleItensMessageDataEvent>(cfgTopology =>
                        {
                            cfgTopology.SetEntityNameFormatter(MessageEntityNameFormatter<SampleItensMessageDataEvent>.Create("message", "sample"));
                        });
                        serviceBusConfigure
                            .SubscriptionEndpoint<SampleItensMessageDataEvent>("sbts-policy_process_sample_itens", endpointConfig =>
                            {
                                endpointConfig.ConfigureConsumer<IConsumer<SampleItensMessageDataEvent>>(context);
                            });
                        
                        serviceBusConfigure.UseNewtonsoftJsonSerializer();

                        serviceBusConfigure.ConfigureNewtonsoftJsonSerializer(settings =>
                        {
                            settings.DefaultValueHandling = DefaultValueHandling.Populate;
                            settings.Converters.Add(new StringEnumConverter());
                            return settings;
                        });
                    });
                });

            return serviceCollection;
        }
    }

    public class MessageEntityNameFormatter<TMessage> : IMessageEntityNameFormatter<TMessage>
        where TMessage : class
    {
        private readonly string _environmentName;
        private readonly string _contexto;

        protected MessageEntityNameFormatter(string environmentName, string contexto)
        {
            if (string.IsNullOrWhiteSpace(environmentName))
            {
                throw new ArgumentException($"'{nameof(environmentName)}' cannot be null or whitespace.", nameof(environmentName));
            }

            if (string.IsNullOrWhiteSpace(contexto))
            {
                throw new ArgumentException($"'{nameof(contexto)}' cannot be null or whitespace.", nameof(contexto));
            }

            _environmentName = environmentName;
            _contexto = contexto;
        }

        public static MessageEntityNameFormatter<TMessage> Create(string environmentName, string contexto)
            => new MessageEntityNameFormatter<TMessage>(environmentName, contexto);

        public string FormatEntityName()
        {
            return $"sbt-{ _environmentName }-{ _contexto }-{ typeof(TMessage).Name }";
        }
    }
}
