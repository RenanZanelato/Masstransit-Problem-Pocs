using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Playground.Messages
{
    public class SampleItensCreatedContract
    {
        public Guid MessageId { get; set; }
        public Guid BatchId { get; set; }
        public IEnumerable<SampleItem> Messages { get; }
        public bool BoolExample { get; }

        [JsonConstructor]
        public SampleItensCreatedContract(
            Guid messageId,
            IEnumerable<SampleItem> messages,
            bool boolexample)
        {
            MessageId = messageId;
            BatchId = BatchId;
            Messages = messages;
            BoolExample = boolexample;
        }
        
        public SampleItensCreatedContract(
            IEnumerable<SampleItem> messages,
            bool boolexample)
        {
            BatchId = BatchId;
            Messages = messages;
            BoolExample = boolexample;
        }
    }
}