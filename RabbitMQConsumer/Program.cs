using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace RabbitMQConsumer
{
    static class Program
    {
        static void Main(string[] args)
        {
            var connectionFactory = new ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672")
            };
            using var connection = connectionFactory.CreateConnection();
            using var channel = connection.CreateModel();
            // QueueConsumer.Consume(channel);
            channel.QueueDeclare("Batch-Start",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, e) =>
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var temp = JsonConvert.DeserializeObject<BatchModel>(message);
                temp.Status = "Complete";
                Console.WriteLine(message);
                PostMessage.Push(temp);
            };

            channel.BasicConsume("Batch-Start", true, consumer);
            Console.ReadLine();
        }
    }
}
