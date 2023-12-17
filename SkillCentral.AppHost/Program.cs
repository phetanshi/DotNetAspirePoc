var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedisContainer("cache");

var skillService = builder.AddProject<Projects.SkillCentral_SkillServices>("skillservices");
var employeeService = builder.AddProject<Projects.SkillCentral_EmployeeServices>("employeeservices");
var notificationService = builder.AddProject<Projects.SkillCentral_NotificationServices>("notificationservices");

builder.AddProject<Projects.SkillCentral>("skillcentralui")
    .WithReference(skillService)
    .WithReference(employeeService)
    .WithReference(notificationService)
    .WithReference(cache);



builder.Build().Run();
