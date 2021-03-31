using System;
using System.Threading.Tasks;

namespace ProducerConsumerExample
{
    public class ItemProducer
    {
        private readonly ItemConsumer _itemConsumer;

        public ItemProducer(ItemConsumer itemConsumer)
        {
            _itemConsumer = itemConsumer;
            _itemConsumer.ConsumptionComplete += OnConsumptionComplete;
        }

        public async Task BeginProduction()
        {
            // Start our consumer task to monitor the collection for new items
            var consumerTask = Task.Run(() => _itemConsumer.Consume());

            // Spool up our producer tasks
            var firstProducer = Task.Run(() => ProduceData(0, 10));
            var secondProducer = Task.Run(() => ProduceData(11, 20));
            var thirdProducer = Task.Run(() => ProduceData(21, 30));

            await Task.WhenAll(firstProducer, secondProducer, thirdProducer);

            // Producers have finished, so mark the collection as complete.
            _itemConsumer.ItemCollection.CompleteAdding();

            // Arbitrary delay just to demo the event message before the console exits.
            await Task.Delay(1000);
        }

        private void ProduceData(int startSeed, int limit)
        {
            for (var i = startSeed; i < limit; i++)
            {
                Console.WriteLine($"Adding item {i} to the blocking collection.");

                // Blocks until items have been removed below the bounded capacity
                // _itemConsumer.ItemCollection.Add(i);

                // Blocks for the period specified in the TryAdd parameter, then returns false if it exceeds this
                var addSuccess = _itemConsumer.ItemCollection.TryAdd(i, 1000);
                if (!addSuccess)
                    Console.WriteLine($"Operation to add item {i} to the blocking collection timed out.");
            }
        }

        private void OnConsumptionComplete(object sender, EventArgs args)
        {
            Console.WriteLine("Adding has been complete, and the consumer has finished iterating.");
        }
    }
}