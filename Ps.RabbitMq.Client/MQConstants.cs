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


    public const string EMPLOYEE_DELETE_ROUTE_KEY = "employee.delete";
    public const string EMPLOYEE_CREATE_ROUTE_KEY = "employee.create";
    public const string EMPLOYEE_BATCH_ROUTE_KEY = "employee.batch";
    public const string EMPLOYEE_UPDATED_ROUTE_KEY = "employee.updated";
    public const string EMPLOYEE_LIKE_ROUTE_KEY = "employee.*";

    public const string EMPLOYEE_SKILL_ADDED_ROUTE_KEY = "employee.skill.add";
    public const string EMPLOYEE_SKILL_DELETED_ROUTE_KEY = "employee.skill.delete";
    public const string EMPLOYEE_SKILL_LIKE_ROUTE_KEY = "employee.skill.*";

    public const string SKILL_ADDED_ROUTE_KEY = "skill.add";
    public const string SKILL_DELETED_ROUTE_KEY = "skill.delete";
    public const string SKILL_UPDATED_ROUTE_KEY = "skill.updated";
    public const string SKILL_LIKE_ROUTE_KEY = "skill.*";

    public const string DEFAULT_DIRECT_ROUTE_KEY = "default_direct";
}
