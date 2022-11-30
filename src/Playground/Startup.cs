using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMassTransitServiceBus();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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

                    var qtdeItensLista = new [] { 1_000, 5_000, 10_00, 25_000, 50_000, 100_000, 200_000, 500_000, 1_000_000 };
                    foreach (var qtde in qtdeItensLista)
                    {
                        var sampleItens = CreateSampleItensMock(qtde).ToArray();
                        var contract = new SampleItensCreatedContract(sampleItens);
                        
                        var @event = new
                        {
                            Id = Guid.NewGuid(),
                            Body = contract
                        };

                        var st = Stopwatch.StartNew();
                        await publisher.Publish<SampleItensMessageDataEvent>(@event);
                        st.Stop();

                        Console.WriteLine($"Event #{@event.Id} published, with items length {contract.Items.Length} - {st.ElapsedMilliseconds}ms");
                    }

                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }

        private static IEnumerable<SampleItem> CreateSampleItensMock(int quantidade)
        {
            for (var i = 0; i < quantidade; i++)
            {
                yield return new SampleItem
                {
                    Id = Guid.NewGuid()
                };
            }
        }
    }
}
