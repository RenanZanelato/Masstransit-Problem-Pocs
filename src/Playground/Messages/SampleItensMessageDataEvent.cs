using System;
using MassTransit;

namespace Playground.Messages
{
    /// <summary>
    /// Sample of event with a large list of objects published using BlobStorage to store de event data
    /// https://masstransit-project.com/usage/message-data.html
    /// </summary>
    public class SampleItensMessageDataEvent
    {
        public Guid Id { get; set; }
        public MessageData<SampleItensCreatedContract> Body { get; set; }
    }
}
