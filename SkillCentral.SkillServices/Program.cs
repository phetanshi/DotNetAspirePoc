using SkillCentral.SkillServices.Apis;
using SkillCentral.SkillServices.Data;
using SkillCentral.Repository;
using SkillCentral.SkillServices.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SkillCentral.SkillServices;
using SkillCentral.SkillServices.Utils;
using SkillCentral.ServiceDefaults;

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

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.MapEmployeeSkillApiEndpoints();
app.MapSkillApiEndpoints();

app.Run();

