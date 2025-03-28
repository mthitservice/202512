using kiconsoleapp.models;
using Microsoft.EntityFrameworkCore;

public class MyDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=172.16.0.2,1433;Database=BZE_TimeKeeper;User Id=BZE;Password=BZE2025!#;TrustServerCertificate=False;Encrypt=False");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<cls_disciplin>().ToTable("Disciplins").HasMany(d => d.Events).WithOne(e => e.disciplin);
        modelBuilder.Entity<cls_event>().ToTable("Events").HasMany(e => e.Records).WithOne(r => r.sportsevent);
        modelBuilder.Entity<cls_record>().ToTable("Records").HasOne(r => r.sportsevent).WithMany(e => e.Records);
    }

    public DbSet<cls_disciplin> Disciplins { get; set; }
    public DbSet<cls_event> Events { get; set; }
    public DbSet<cls_record> Records { get; set; }
}