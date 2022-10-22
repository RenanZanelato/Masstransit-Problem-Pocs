using System;
using System.Diagnostics;
using System.Threading.Tasks;
using MassTransit;
using Playground.Messages;

namespace Playground.Consumer
{
    public sealed class SampleCreatedConsumer : IConsumer<SampleCreatedEvent>
    {
        public async Task Consume(ConsumeContext<SampleCreatedEvent> context)
        {
            var st = Stopwatch.StartNew();
            Console.WriteLine($"Consumed event #{nameof(SampleCreatedEvent)} Message #{context.MessageId} = {context.Message.MessageId} with description {context.Message.Description} {st.ElapsedMilliseconds}ms");
            st.Stop();
        }
    }
}
