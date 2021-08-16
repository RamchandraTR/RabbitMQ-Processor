using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQConsumer
{
    public static class PostMessage
    {
        public static void Push(BatchModel model)
        {
            var connectionFactory = new ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672")
            };
            using var connection = connectionFactory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare("Batch-Complete",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            var message = new { Name = "Ram", message = "" };
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(model));

            channel.BasicPublish("", "Batch-Complete", null, body);
        }
    }
}
