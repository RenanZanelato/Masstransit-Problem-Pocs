using System;

namespace Playground.Messages
{
    public class SampleCreatedEvent
    {
        public Guid MessageId { get; set; }
        public string Description { get; set; }
    }
}
