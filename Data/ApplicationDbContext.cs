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

    public void SeedData()
    {
        if (Suppliers.Any()) return;

        Suppliers.AddRange(
            new Supplier { Name = "Fornecedor Teste 1", Document = "123456789", Active = true },
            new Supplier { Name = "Fornecedor Teste 2", Document = "987654321", Active = true }
        );

        SaveChanges();
    }
}