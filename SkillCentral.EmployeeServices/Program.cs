using SkillCentral.EmployeeServices.Apis;
using SkillCentral.EmployeeServices.Data;
using SkillCentral.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSqlServerRepository<EmployeeDbContext>(builder.Configuration);
var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapEmployeeApiEndpoints();
app.Run();
