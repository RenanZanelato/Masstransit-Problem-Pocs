using System;
using MassTransit;
using Newtonsoft.Json;

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

    public class SampleItensCreatedContract
    {
        public SampleItem[] Items { get; }

        [JsonConstructor]
        public SampleItensCreatedContract(SampleItem[] items)
        {
            Items = items;
        }
    }

    [Serializable]
    public class SampleItem
    {
        public Guid Id { get; set; }
    }
}
