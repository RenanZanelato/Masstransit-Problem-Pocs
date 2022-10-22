using System;
using System.Diagnostics;
using System.Threading.Tasks;
using MassTransit;
using Playground.Messages;

namespace Playground.Consumer
{
    public sealed class SampleDeletedConsumer : IConsumer<SampleDeletedEvent>
    {
        public async Task Consume(ConsumeContext<SampleDeletedEvent> context)
        {
            var st = Stopwatch.StartNew();
            Console.WriteLine($"Consumed event #{nameof(SampleDeletedEvent)} Message #{context.MessageId} = {context.Message.MessageId} {st.ElapsedMilliseconds}ms");
            st.Stop();
        }
    }
}
