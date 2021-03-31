using System;
using System.Threading.Tasks;

namespace ProducerConsumerExample
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var itemProducer = new ItemProducer(new ItemConsumer());
            await itemProducer.BeginProduction();

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}