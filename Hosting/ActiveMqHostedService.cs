using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using System.Threading;
using Amqp;
using Amqp.Framing;
using Amqp.Types;

namespace temp_artemis
{
    public class ActiveMqHostedService:IHostedService{
        private CancellationTokenSource _cts;
        private Task _task;
        private Address _address;
        private Connection _connection;

        public ActiveMqHostedService()
        {
            _address = new Address("amqp://guest:guest@localhost:5672");

        }

        public async Task StartAsync(CancellationToken token)
        {
            _connection = await new ConnectionFactory().CreateAsync(_address).ConfigureAwait(false);
            _cts = CancellationTokenSource.CreateLinkedTokenSource(token);
            var lcl_token = _cts.Token;
            var session = ((IConnection) _connection ).CreateSession();
             _task = Task.Run( async () => {
                while(!token.IsCancellationRequested)
                {
                    var receiver = session.CreateReceiver( "udai-test",
                                                            new Source
                                                                {
                                                                   Address = "Udai-Created-1",
                                                                   Capabilities = new[] { new Symbol( "queue" ) }
                                                               } );
                    var msg = await receiver.ReceiveAsync().ConfigureAwait(false);
                    if (msg != null){
                        System.Console.WriteLine($"Received message ${msg}");
                    }
                }
             }, token);
        }

        public async Task StopAsync(CancellationToken token)
        {
            
        }

    }
}