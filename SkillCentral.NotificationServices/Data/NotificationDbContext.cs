using Microsoft.EntityFrameworkCore;
using SkillCentral.NotificationServices.Data.DbModels;

namespace SkillCentral.NotificationServices.Data;

public class NotificationDbContext : DbContext
{
    public NotificationDbContext(DbContextOptions<NotificationDbContext> opt) : base(opt)
    {

    }

    public DbSet<Notification> Notifications { get; set; }
}
