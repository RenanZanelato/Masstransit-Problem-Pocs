using System;
using System.Diagnostics;
using System.Threading.Tasks;
using MassTransit;
using Playground.Messages;

namespace Playground.Consumer
{
    public sealed class SampleUpdatedConsumer : IConsumer<SampleUpdatedEvent>
    {
        public async Task Consume(ConsumeContext<SampleUpdatedEvent> context)
        {
            var st = Stopwatch.StartNew();
            Console.WriteLine($"Consumed event #{nameof(SampleUpdatedEvent)} Message #{context.MessageId} = {context.Message.MessageId} with description {context.Message.Description} {st.ElapsedMilliseconds}ms");
            st.Stop();
        }
    }
}
