using Microsoft.EntityFrameworkCore;
using SkillCentral.SkillServices.Data.DbModels;

namespace SkillCentral.SkillServices.Data;

public class SkillDbContext : DbContext
{
    public SkillDbContext(DbContextOptions<SkillDbContext> opt) : base(opt)
    {

    }

    public DbSet<Skill> Skills { get; set; }
    public DbSet<EmployeeSkill> EmployeeSkills { get; set; }
}