using System;
using System.Diagnostics;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Playground.Infra;
using Playground.Messages;

namespace Playground
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMassTransitServiceBus();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    var publisher = context.RequestServices.GetRequiredService<IPublishEndpoint>();
                    var st = Stopwatch.StartNew();

                    var createdEvent = new SampleCreatedEvent()
                    {
                        MessageId = Guid.NewGuid(),
                        Description = "Example-Sample-Created-Event"
                    };
                    await publisher.Publish(createdEvent);

                    Console.WriteLine(
                        $"Publishing event ${nameof(SampleCreatedEvent)} #{createdEvent.MessageId} with {createdEvent.MessageId} {st.ElapsedMilliseconds}ms");

                    var updatedEvent = new SampleUpdatedEvent()
                    {
                        MessageId = Guid.NewGuid(),
                        Description = "Example-Sample-Updated-Event"
                    };
                    await publisher.Publish(updatedEvent);

                    Console.WriteLine(
                        $"Publishing event ${nameof(SampleUpdatedEvent)} #{updatedEvent.MessageId} with {updatedEvent.MessageId} {st.ElapsedMilliseconds}ms");

                    var deletedEvent = new SampleDeletedEvent()
                    {
                        MessageId = Guid.NewGuid(),
                    };
                    await publisher.Publish(deletedEvent);

                    Console.WriteLine(
                        $"Publishing event ${nameof(SampleDeletedEvent)} #{deletedEvent.MessageId} with {deletedEvent.MessageId} {st.ElapsedMilliseconds}ms");

                    st.Stop();
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}