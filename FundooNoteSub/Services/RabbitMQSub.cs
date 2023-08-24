using FundooNoteSub.Interface;
using FundooNoteSub.Model;
using MassTransit;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace FundooNoteSub.Services
{
    public class RabbitMQSub : IRabbitMQSub
    {
        private readonly ConnectionFactory factory;
        private readonly IConfiguration configuration;
        private readonly IBusControl _busControl; // Add this field to inject MassTransit bus

        public RabbitMQSub(ConnectionFactory _factory, IConfiguration _configuration, IBusControl busControl)
        {
            factory = _factory;
            configuration = _configuration;
            _busControl = busControl; // Inject the MassTransit bus

            // Start consuming messages
            ConsumeMessages();
        }

        public void ConsumeMessages()
        {
            using (var connection = factory.CreateConnection())
            {
                Console.WriteLine("Connection to RabbitMQ server established");

                using (var channel = connection.CreateModel())
                {
                    var queueName = "User-Registration-Queue";
                    channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += async (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);

                        // Send the received email to the UserRegistrationEmailSubscriber consumer
                        await _busControl.Publish<UserRegMsg>(new
                        {
                            Email = message
                        });
                    };

                    channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
                }
            }
        }

    }
}
