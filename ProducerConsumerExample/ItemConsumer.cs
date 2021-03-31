using System;
using System.Collections.Concurrent;

namespace ProducerConsumerExample
{
    public class ItemConsumer
    {
        public EventHandler<EventArgs> ConsumptionComplete;

        public ItemConsumer()
        {
            ItemCollection = new BlockingCollection<int>();
        }

        public BlockingCollection<int> ItemCollection { get; }

        public void Consume()
        {
            foreach (var item in ItemCollection.GetConsumingEnumerable())
                Console.WriteLine($"Consuming and removing item {item}");

            // Now let any subscribers know that we have finished consuming items.
            ConsumptionComplete?.Invoke(this, EventArgs.Empty);
        }
    }
}