using FundooNoteSub.Services;
using MassTransit;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;

namespace FundooNoteSub
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
               .AddJsonFile(@"C:\Users\91951\source\repos\FundooNoteApp\FundooNoteSub\appsettings.json", optional: false)
               .Build();

            var factory = new ConnectionFactory
            {
                HostName = configuration["RabbitMQSettings:HostName"],
                UserName = configuration["RabbitMQSettings:UserName"],
                Password = configuration["RabbitMQSettings:Password"]
            };

            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(new Uri(configuration["RabbitMQSettings:HostUri"]), h =>
                {
                    h.Username(configuration["RabbitMQSettings:UserName"]);
                    h.Password(configuration["RabbitMQSettings:Password"]);
                });

                // Automatically register the consumer
                cfg.ReceiveEndpoint("User-Registration-Queue", e =>
                {
                    // Automatically register the consumer using the DI container
                    e.Consumer<UserRegEmailSub>();
                });
            });

            var subscriber = new RabbitMQSub(factory, configuration, busControl);
            subscriber.ConsumeMessages();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
    
}
