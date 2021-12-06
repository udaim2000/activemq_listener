using System;
using Amqp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace temp_artemis
{
    public class ActiveMQConnectionFactory
    {
        private readonly ILogger _logger;

        public ActiveMQConnectionFactory(ILogger<ActiveMQConnectionFactory> logger)
        {
            _logger = logger;
        }

        public async Task<Connection> CreateConnection()
        {   var address = new Address("amqp://guest:guest@localhost:5672");
            var ConnectionFactory =  new ConnectionFactory();
            return await ConnectionFactory.CreateAsync(address);
        }
    }
}