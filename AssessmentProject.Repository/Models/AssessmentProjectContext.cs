using Microsoft.EntityFrameworkCore;

namespace AssessmentProject.Repository.Models;

public class AssessmentProjectContext : DbContext
{
    public AssessmentProjectContext()
    {
    }

    public AssessmentProjectContext(DbContextOptions<AssessmentProjectContext> options)
        : base(options)
    {
    }
    public DbSet<Users> Users { get; set; }
    public DbSet<Roles> Roles { get; set; }
    public DbSet<Countries> Countries { get; set; }
    public DbSet<States> States { get; set; }
    public DbSet<Cities> Cities { get; set; }
    public DbSet<Concerts> Concerts { get; set; }
    public DbSet<UserConcertsMapping> UserConcertsMapping { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=localhost;Database=concertDB;Username=postgres;password=Tatva@123");
}
