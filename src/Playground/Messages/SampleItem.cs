using System;
using Newtonsoft.Json;

namespace Playground.Messages
{
    [Serializable]
    public class SampleItem
    {
        public Guid Id { get; }

        public SampleItem(Guid id)
        {
            Id = id;
        }
    }
}