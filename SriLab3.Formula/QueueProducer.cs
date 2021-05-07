using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SriLab3.Formula
{
    public static class QueueProducer
    {
        public static void Publish(IModel channel, IModel channel2, IModel channel3, string queueName, string queueName2)
        {
            channel.QueueDeclare(queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            channel2.QueueDeclare(queueName,
              durable: true,
              exclusive: false,
              autoDelete: false,
              arguments: null);

            channel3.QueueDeclare("formula-queue",
           durable: true,
           exclusive: false,
           autoDelete: false,
           arguments: null);

            var count = 0;

            while (true)
            {
                var message = new { TemperaturaOleju = 95+count, CisnienieOpon = 95+count, Wiadomosc = "", Czas = DateTime.Now };
                if (count % 10 == 0)
                {
                    message = new { TemperaturaOleju = 95 + count, CisnienieOpon = 95 + count, Wiadomosc = "Proszę o zjazd", Czas = DateTime.Now };
                    var consumer = new EventingBasicConsumer(channel3);
                    consumer.Received += (sender, e) =>
                    {
                        var body1 = e.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body1);
                        Console.WriteLine(message);
                    };
                    channel.BasicConsume("formula-queue", true, consumer);
                    count = count - 10;
                }
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
                
                channel.BasicPublish("", queueName, null, body);
                channel2.BasicPublish("", queueName2, null, body);
                count++;
                Thread.Sleep(1000);
            }
        }
    }
}
