using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading;

namespace SriLab3.Formula
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
            using var channel1 = connection.CreateModel();
            using var channel2 = connection.CreateModel();
            QueueProducer.Publish(channel, channel1,channel2,"pitstop-queue","logger-queue");
           
        }
    }
}
