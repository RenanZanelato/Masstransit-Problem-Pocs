using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Playground.Messages;

namespace Playground.Consumer
{
    public sealed class SampleItensCreatedConsumer : IConsumer<SampleItensMessageDataEvent>
    {
        public async Task Consume(ConsumeContext<SampleItensMessageDataEvent> context)
        {
            var st = Stopwatch.StartNew();
            var message = await context.Message.Body.Value;
            st.Stop();

            Console.WriteLine($"Message #{context.MessageId}={context.Message.Id} consumed, with items totalMessages {message.Messages.Count()} - {st.ElapsedMilliseconds}ms");
        }
    }
}
