using DemoMinimalAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoMinimalAPI.Data;

public class ApplicationDbContext : DbContext
{
   public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Supplier> Suppliers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Supplier>()
            .ToTable("Suppliers");

        modelBuilder.Entity<Supplier>()
            .HasKey(s => s.Id);

        modelBuilder.Entity<Supplier>()
            .Property(s => s.Name)
            .IsRequired();

        modelBuilder.Entity<Supplier>()
            .Property(s => s.Document);

        modelBuilder.Entity<Supplier>()
            .Property(s => s.Active)
            .IsRequired();
     
        base.OnModelCreating(modelBuilder);
    }
}