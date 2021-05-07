using RabbitMQ.Client;
using System;

namespace SriLab3.Logger
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672")
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            //  channel.QueueDeclare("pitstop-queue", durable: true, exclusive: false,autoDelete:false,arguments:null);

            QueueConsumer.Consume(channel);
        }
    }
}
