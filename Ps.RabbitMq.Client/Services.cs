using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ps.RabbitMq.Client
{
    public static class Services
    {
        public static void AddRabbitMqCustomClient(this IHostApplicationBuilder builder, string name)
        {
            builder.AddRabbitMQ(name);
            builder.Services.AddSingleton<MqUtil>();
            builder.Services.AddSingleton<IMqPubSubService, MqPubSubService>();
            builder.Services.AddSingleton<IMqRequestService, MqRequestService>();
        }
    }
}
