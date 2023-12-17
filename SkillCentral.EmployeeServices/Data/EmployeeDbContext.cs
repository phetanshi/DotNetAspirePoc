using Microsoft.EntityFrameworkCore;
using SkillCentral.EmployeeServices.Data.DbModels;
using System.Globalization;

namespace SkillCentral.EmployeeServices.Data;

public class EmployeeDbContext : DbContext
{
    public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options)
    {
        
    }
    public DbSet<Employee> Employees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
        CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
        base.OnModelCreating(modelBuilder);
    }

}
