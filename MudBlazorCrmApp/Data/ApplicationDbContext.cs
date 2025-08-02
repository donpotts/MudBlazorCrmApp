using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MudBlazorCrmApp.Models;
using MudBlazorCrmApp.Shared.Models;

namespace MudBlazorCrmApp.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Customer> Customer => Set<Customer>();
    public DbSet<Address> Address => Set<Address>();
    public DbSet<ProductCategory> ProductCategory => Set<ProductCategory>();
    public DbSet<ServiceCategory> ServiceCategory => Set<ServiceCategory>();
    public DbSet<Contact> Contact => Set<Contact>();
    public DbSet<Opportunity> Opportunity => Set<Opportunity>();
    public DbSet<Lead> Lead => Set<Lead>();
    public DbSet<Product> Product => Set<Product>();
    public DbSet<Service> Service => Set<Service>();
    public DbSet<Sale> Sale => Set<Sale>();
    public DbSet<Vendor> Vendor => Set<Vendor>();
    public DbSet<SupportCase> SupportCase => Set<SupportCase>();
    public DbSet<TodoTask> TodoTask => Set<TodoTask>();
    public DbSet<Reward> Reward => Set<Reward>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Customer>()
            .HasMany(x => x.Address);
        modelBuilder.Entity<Customer>()
            .HasMany(x => x.Contact);
        modelBuilder.Entity<Contact>()
            .HasMany(x => x.Address);
        modelBuilder.Entity<Contact>()
            .HasMany(x => x.Reward);
        modelBuilder.Entity<Lead>()
            .Property(e => e.PotentialValue)
            .HasPrecision(19, 4);
        modelBuilder.Entity<Lead>()
            .HasMany(x => x.Address);
        modelBuilder.Entity<Lead>()
            .HasMany(x => x.Opportunity);
        modelBuilder.Entity<Lead>()
            .HasMany(x => x.Contact);
        modelBuilder.Entity<Product>()
            .Property(e => e.Price)
            .HasPrecision(19, 4);
        modelBuilder.Entity<Product>()
            .HasMany(x => x.ProductCategory);
        modelBuilder.Entity<Service>()
            .HasMany(x => x.ServiceCategory);
        modelBuilder.Entity<Sale>()
            .Property(e => e.TotalAmount)
            .HasPrecision(19, 4);
        modelBuilder.Entity<Vendor>()
            .HasMany(x => x.Address);
        modelBuilder.Entity<Vendor>()
            .HasMany(x => x.Product);
        modelBuilder.Entity<Vendor>()
            .HasMany(x => x.Service);
        modelBuilder.Entity<Reward>()
            .Property(e => e.CreditsDollars)
            .HasPrecision(19, 4);
        modelBuilder.Entity<Reward>()
            .Property(e => e.ConversionRate)
            .HasPrecision(19, 4);
    }
}
