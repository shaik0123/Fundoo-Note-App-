using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepoLayer.Services
{
    public class MessageService
    {
        private readonly IConfiguration _configuration;
        private readonly ServiceBusClient _client;
        private readonly ServiceBusSender _sender;
        public MessageService(IConfiguration configuration, ServiceBusClient client, ServiceBusSender sender)
        {
            _configuration = configuration;
            _client = client;
            _sender = sender;

            /*string connectionString = _configuration.GetConnectionString("AzureServiceBusConnectionString");
            string queueName = _configuration["AzureServiceBusQueue"];

            _client = new ServiceBusClient(connectionString);
            _sender = _client.CreateSender(queueName);*/
        }
        public async Task SendMsgToQueue(string email,string token) 
        {
            string messageBody = "Token : "+token;
            ServiceBusMessage message = new ServiceBusMessage();
            message.Subject = " Password Reset Token";
            message.To = email;
            message.Body = BinaryData.FromString(messageBody);
            _sender.SendMessageAsync(message).Wait(); 
        }
        
    }
}
