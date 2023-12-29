using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ps.RabbitMq.Client;

public static class MQConstants
{
    public const string FANOUT_EXCHANGE_NAME = "app-fanout-exchange";
    public const string TOPIC_EXCHANGE_NAME = "app-topic-exchange";
    public const string DIRECT_EXCHANGE_NAME = "app-direct-exchange";

    public const string DELETE_EMPLOYEE = "delete.employee";
}
