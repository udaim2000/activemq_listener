using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Amqp;
using Microsoft.Extensions.Logging;
using Amqp.Framing;
using Amqp.Types;

namespace temp_artemis
{
    public class AMQPListener : IHostedService, IDisposable
    {
        private Task _executingTask;
        private readonly ActiveMQConnectionFactory _amqpConnectionFactory;
        private readonly ILogger _logger;
        private Connection _connection;
        public AMQPListener(ILogger<AMQPListener> logger, ActiveMQConnectionFactory amqpConnectionFactory)
        {
            _amqpConnectionFactory = amqpConnectionFactory;
            _logger = logger;
        }

        public Task StartAsync(System.Threading.CancellationToken token)
        {
            _connection =  _amqpConnectionFactory.CreateConnection().Result;
        _executingTask = ExecuteAsync(token);

        // If the task is completed then return it,
        // this will bubble cancellation and failure to the caller
        if (_executingTask.IsCompleted)
        {
            return _executingTask;
        }

        // Otherwise it's running
        return Task.CompletedTask;
        }

        public async Task ExecuteAsync(CancellationToken token)
        {
            var session = ( (IConnection) _connection ).CreateSession();
            var receiver = session.CreateReceiver( $"Udai_Created_2",
                                                               new Source
                                                               {
                                                                   Address = "Udai_Created_2",
                                                                   Capabilities = new[] { new Symbol( "queue" ) }
                                                               } );        
            while(!token.IsCancellationRequested){    
                var msg = await receiver.ReceiveAsync().ConfigureAwait(false);
                Console.WriteLine($"Message Recieved: {msg}");
            }
        }
        public Task StopAsync(CancellationToken token)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {

        }
    }
}