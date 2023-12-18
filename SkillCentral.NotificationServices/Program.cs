using Microsoft.Extensions.DependencyInjection.Extensions;
using SkillCentral.NotificationServices.Apis;
using SkillCentral.NotificationServices.Data;
using SkillCentral.NotificationServices.Services;
using SkillCentral.Repository;

namespace SkillCentral.NotificationServices;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddSqlServerRepository<NotificationDbContext>(builder.Configuration);
        builder.Services.AddScoped<INotificationService, InAppNotificationService>();
        builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        var app = builder.Build();

        app.MapDefaultEndpoints();

        // Configure the HTTP request pipeline.
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapNotificationApiEndpoints();

        app.Run();
    }
}
