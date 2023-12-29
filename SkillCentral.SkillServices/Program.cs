using Microsoft.Extensions.DependencyInjection.Extensions;
using SkillCentral.Repository;
using SkillCentral.SkillServices.Apis;
using SkillCentral.SkillServices.Contracts;
using SkillCentral.SkillServices.Data;
using SkillCentral.SkillServices.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSqlServerRepository<SkillDbContext>(builder.Configuration);

builder.Services.AddScoped<ISkillService, SkillService>();
builder.Services.AddScoped<IEmployeeSkillService, EmployeeSkillService>();
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddSingleton<EmployeeMQContract>();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();



app.UseHttpsRedirection();
app.MapEmployeeSkillApiEndpoints();
app.MapSkillApiEndpoints();

app.Services.GetService<EmployeeMQContract>().InvokeAsync();
app.Run();


//app.Services.CreateScope()
//                    .ServiceProvider
//                    .GetService<EmployeeMQContract>()
//                    .InvokeAsync();