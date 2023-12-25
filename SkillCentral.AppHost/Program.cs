var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedisContainer("cache");
var messaging = builder.AddRabbitMQContainer("messaging", 15672);

var skillService = builder.AddProject<Projects.SkillCentral_SkillServices>("skillservices")
    .WithReference(messaging);
var employeeService = builder.AddProject<Projects.SkillCentral_EmployeeServices>("employeeservices")
    .WithReference(messaging);

var notificationService = builder.AddProject<Projects.SkillCentral_NotificationServices>("notificationservices");

builder.AddProject<Projects.SkillCentral>("skillcentralui")
    .WithReference(skillService)
    .WithReference(employeeService)
    .WithReference(notificationService)
    .WithReference(cache);



builder.Build().Run();
