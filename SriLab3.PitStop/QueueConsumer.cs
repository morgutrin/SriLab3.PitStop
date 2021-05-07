using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SriLab3.PitStop
{
    public static class QueueConsumer
    {

        public static void Consume(IModel channel)
        {
            channel.QueueDeclare("pitstop-queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            channel.QueueDeclare("mechanic-queue",
             durable: true,
             exclusive: false,
             autoDelete: false,
             arguments: null);
            channel.QueueDeclare("formula-queue",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

            var message = new { Wiadomosc = "Szykujcie sie na robote" };
            var message2 = new { Wiadomosc = "Możesz zjechać" };
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
         
          
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, e) => {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
               // Console.WriteLine(message);
                var jsonMessage = JsonConvert.DeserializeObject<FormulaData>(message);
                if(jsonMessage.Wiadomosc.Equals("Proszę o zjazd"))
                {
                    Console.WriteLine("Formula wysłała prośbę o zjazd");
                    channel.BasicPublish("", "formula-queue", null, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message2)));
                    Console.WriteLine("Zgoda na zjazd");
                }
                if(jsonMessage.CisnienieOpon==95 || jsonMessage.TemperaturaOleju == 95)
                {
                    channel.BasicPublish("", "mechanic-queue", null, body);
                    Console.WriteLine("Wysłano powiadomienie do mechaników");
                  
                }
            };
            
            channel.BasicConsume("pitstop-queue", true, consumer);
            Console.WriteLine("Consumer started");
            Console.ReadLine();
        }
    }
}
