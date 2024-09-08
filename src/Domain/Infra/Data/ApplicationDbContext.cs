using Flunt.Notifications;
using IWantApp.Domain.Products;
using Microsoft.EntityFrameworkCore;

namespace IWantApp.Domain.Infra.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Ignore<Notification>();

        builder.Entity<Product>()
            .Property(p => p.Description)
            .HasMaxLength(255);

        builder.Entity<Product>()
            .Property(p => p.Name)
            .IsRequired();

        builder.Entity<Category>()
            .Property(p => p.Name)
            .IsRequired();
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<string>()
            .HaveMaxLength(100);
    }
}
