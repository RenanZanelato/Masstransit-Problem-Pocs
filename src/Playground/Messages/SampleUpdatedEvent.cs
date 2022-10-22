using System;

namespace Playground.Messages
{
    public class SampleUpdatedEvent
    {
        public Guid MessageId { get; set; }
        public string Description { get; set; }
    }
}
