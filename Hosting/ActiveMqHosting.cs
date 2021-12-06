using Microsoft.Extensions.DependencyInjection;

namespace temp_artemis{


    public static class ActiveMqHostingExtensions
    {
        public static IServiceCollection AddActiveMqHostedService(this IServiceCollection services)
        {
            // return services.AddHostedService<ActiveMqHostedService>();
            return services.AddHostedService<AMQPListener>();
        }
    }
}