using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQConsumer
{
    public static class QueueConsumer
    {
        public static void Consume(IModel channel)
        {
            channel.QueueDeclare("Ram-Queue",
               durable: true,
               exclusive: false,
               autoDelete: false,
               arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, e) =>
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(message);
                // PostMessage.Push("I'm from producer");
            };

            channel.BasicConsume("Ram-Queue", true, consumer);
            Console.WriteLine("Consumer Started");
            Console.ReadLine();
        }
    }
}
