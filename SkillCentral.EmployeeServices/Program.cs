using Microsoft.Extensions.DependencyInjection.Extensions;
using SkillCentral.EmployeeServices.Apis;
using SkillCentral.EmployeeServices.Contracts;
using SkillCentral.EmployeeServices.Data;
using SkillCentral.EmployeeServices.Services;
using SkillCentral.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSqlServerRepository<EmployeeDbContext>(builder.Configuration);

builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IFileService, FileService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddHostedService<EmployeeHostedService>();

builder.Services.AddAntiforgery();
var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapEmployeeApiEndpoints();

app.Run();

