using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MudBlazorCrmApp.Models;
using MudBlazorCrmApp.Shared.Models;
using System.Text.Json;

namespace MudBlazorCrmApp.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    private string? _currentUserId;
    private string? _currentUserName;

    /// <summary>
    /// Sets the current user context for audit tracking
    /// </summary>
    public void SetCurrentUser(string? userId, string? userName)
    {
        _currentUserId = userId;
        _currentUserName = userName;
    }

    public DbSet<Customer> Customer => Set<Customer>();

    public override int SaveChanges()
    {
        var auditEntries = OnBeforeSaveChanges();
        UpdateTimestamps();
        var result = base.SaveChanges();
        OnAfterSaveChanges(auditEntries);
        return result;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var auditEntries = OnBeforeSaveChanges();
        UpdateTimestamps();
        var result = await base.SaveChangesAsync(cancellationToken);
        await OnAfterSaveChangesAsync(auditEntries, cancellationToken);
        return result;
    }

    private List<AuditEntry> OnBeforeSaveChanges()
    {
        ChangeTracker.DetectChanges();
        var auditEntries = new List<AuditEntry>();
        var correlationId = Guid.NewGuid().ToString();

        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is AuditLog || entry.Entity is ActivityLog ||
                entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                continue;

            var auditEntry = new AuditEntry(entry)
            {
                TableName = entry.Entity.GetType().Name,
                UserId = _currentUserId,
                UserName = _currentUserName,
                CorrelationId = correlationId
            };

            auditEntries.Add(auditEntry);

            foreach (var property in entry.Properties)
            {
                if (property.IsTemporary)
                {
                    auditEntry.TemporaryProperties.Add(property);
                    continue;
                }

                string propertyName = property.Metadata.Name;
                if (property.Metadata.IsPrimaryKey())
                {
                    auditEntry.KeyValues[propertyName] = property.CurrentValue;
                    continue;
                }

                switch (entry.State)
                {
                    case EntityState.Added:
                        auditEntry.ChangeType = AuditChangeTypes.Insert;
                        auditEntry.NewValues[propertyName] = property.CurrentValue;
                        break;

                    case EntityState.Deleted:
                        auditEntry.ChangeType = AuditChangeTypes.Delete;
                        auditEntry.OldValues[propertyName] = property.OriginalValue;
                        break;

                    case EntityState.Modified:
                        if (property.IsModified && !Equals(property.OriginalValue, property.CurrentValue))
                        {
                            auditEntry.ChangedProperties.Add(propertyName);
                            auditEntry.ChangeType = AuditChangeTypes.Update;
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            auditEntry.NewValues[propertyName] = property.CurrentValue;

                            // Check if this is a soft delete
                            if (propertyName == nameof(ISoftDelete.IsDeleted) && 
                                property.CurrentValue is true)
                            {
                                auditEntry.ChangeType = AuditChangeTypes.SoftDelete;
                            }
                            else if (propertyName == nameof(ISoftDelete.IsDeleted) && 
                                     property.CurrentValue is false &&
                                     property.OriginalValue is true)
                            {
                                auditEntry.ChangeType = AuditChangeTypes.Restore;
                            }
                        }
                        break;
                }
            }
        }

        // Save audit entries that have temporary properties
        foreach (var auditEntry in auditEntries.Where(e => !e.HasTemporaryProperties))
        {
            AuditLog.Add(auditEntry.ToAuditLog());
        }

        return auditEntries.Where(e => e.HasTemporaryProperties).ToList();
    }

    private void OnAfterSaveChanges(List<AuditEntry> auditEntries)
    {
        if (auditEntries == null || auditEntries.Count == 0)
            return;

        foreach (var auditEntry in auditEntries)
        {
            foreach (var prop in auditEntry.TemporaryProperties)
            {
                if (prop.Metadata.IsPrimaryKey())
                {
                    auditEntry.KeyValues[prop.Metadata.Name] = prop.CurrentValue;
                }
                else
                {
                    auditEntry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
                }
            }
            AuditLog.Add(auditEntry.ToAuditLog());
        }
        base.SaveChanges();
    }

    private async Task OnAfterSaveChangesAsync(List<AuditEntry> auditEntries, CancellationToken cancellationToken)
    {
        if (auditEntries == null || auditEntries.Count == 0)
            return;

        foreach (var auditEntry in auditEntries)
        {
            foreach (var prop in auditEntry.TemporaryProperties)
            {
                if (prop.Metadata.IsPrimaryKey())
                {
                    auditEntry.KeyValues[prop.Metadata.Name] = prop.CurrentValue;
                }
                else
                {
                    auditEntry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
                }
            }
            AuditLog.Add(auditEntry.ToAuditLog());
        }
        await base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            // Handle IAuditableEntity
            if (entry.Entity is IAuditableEntity auditableEntity)
            {
                if (entry.State == EntityState.Added)
                {
                    auditableEntity.CreatedDate ??= DateTime.UtcNow;
                    auditableEntity.CreatedBy ??= _currentUserId;
                }
                auditableEntity.ModifiedDate = DateTime.UtcNow;
                auditableEntity.ModifiedBy = _currentUserId;
            }
            // Handle non-auditable entities (backward compatibility)
            else if (entry.Entity is Customer customer)
            {
                if (entry.State == EntityState.Added)
                    customer.CreatedDate ??= DateTime.UtcNow;
                customer.ModifiedDate = DateTime.UtcNow;
            }
            else if (entry.Entity is Lead lead)
            {
                if (entry.State == EntityState.Added)
                    lead.CreatedDate ??= DateTime.UtcNow;
                lead.ModifiedDate = DateTime.UtcNow;
            }
            else if (entry.Entity is Opportunity opportunity)
            {
                if (entry.State == EntityState.Added)
                    opportunity.CreatedDate ??= DateTime.UtcNow;
                opportunity.ModifiedDate = DateTime.UtcNow;
            }
            else if (entry.Entity is Sale sale)
            {
                if (entry.State == EntityState.Added)
                    sale.CreatedDate ??= DateTime.UtcNow;
                sale.ModifiedDate = DateTime.UtcNow;
            }
            else if (entry.Entity is Contact contact)
            {
                if (entry.State == EntityState.Added)
                    contact.CreatedDate ??= DateTime.UtcNow;
                contact.ModifiedDate = DateTime.UtcNow;
            }
            else if (entry.Entity is SupportCase supportCase)
            {
                if (entry.State == EntityState.Added)
                    supportCase.CreatedDate ??= DateTime.UtcNow;
                supportCase.ModifiedDate = DateTime.UtcNow;
            }
            else if (entry.Entity is Communication communication)
            {
                if (entry.State == EntityState.Added)
                    communication.CreatedDate = DateTime.UtcNow;
                communication.ModifiedDate = DateTime.UtcNow;
            }
            else if (entry.Entity is ActivityLog activityLog && entry.State == EntityState.Added)
            {
                activityLog.Timestamp = DateTime.UtcNow;
            }
        }
    }

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
    public DbSet<ActivityLog> ActivityLog => Set<ActivityLog>();
    public DbSet<Communication> Communication => Set<Communication>();
    public DbSet<AuditLog> AuditLog => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure global query filters for soft delete
        modelBuilder.Entity<Customer>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Contact>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Lead>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Opportunity>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Sale>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<SupportCase>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Communication>().HasQueryFilter(e => !e.IsDeleted);

        // Customer relationships
        modelBuilder.Entity<Customer>()
            .HasOne(x => x.Address);
        modelBuilder.Entity<Customer>()
            .HasMany(x => x.Contact);

        // Contact relationships
        modelBuilder.Entity<Contact>()
            .HasOne(x => x.Address);
        modelBuilder.Entity<Contact>()
            .HasMany(x => x.Reward);
        modelBuilder.Entity<Contact>()
            .HasOne(x => x.Customer)
            .WithMany()
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.SetNull);

        // Lead relationships and configuration
        modelBuilder.Entity<Lead>()
            .Property(e => e.PotentialValue)
            .HasConversion<double>();
        modelBuilder.Entity<Lead>()
            .Property(e => e.PotentialValue)
            .HasPrecision(19, 4);
        modelBuilder.Entity<Lead>()
            .HasOne(x => x.Address);
        modelBuilder.Entity<Lead>()
            .HasOne(x => x.Opportunity);
        modelBuilder.Entity<Lead>()
            .HasOne(x => x.Contact);
        modelBuilder.Entity<Lead>()
            .HasOne(x => x.ConvertedToCustomer)
            .WithMany()
            .HasForeignKey(x => x.ConvertedToCustomerId)
            .OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<Lead>()
            .HasIndex(x => x.Status);
        modelBuilder.Entity<Lead>()
            .HasIndex(x => x.AssignedUserId);

        // Opportunity relationships and configuration
        modelBuilder.Entity<Opportunity>()
            .Property(e => e.Value)
            .HasConversion<double>();
        modelBuilder.Entity<Opportunity>()
            .Property(e => e.Value)
            .HasPrecision(19, 4);
        modelBuilder.Entity<Opportunity>()
            .HasOne(x => x.Customer)
            .WithMany()
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<Opportunity>()
            .HasOne(x => x.Contact)
            .WithMany()
            .HasForeignKey(x => x.ContactId)
            .OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<Opportunity>()
            .HasIndex(x => x.Stage);
        modelBuilder.Entity<Opportunity>()
            .HasIndex(x => x.AssignedUserId);

        // Product relationships and configuration
        modelBuilder.Entity<Product>()
            .Property(e => e.Price)
            .HasConversion<double>();
        modelBuilder.Entity<Product>()
            .Property(e => e.Price)
            .HasPrecision(19, 4);
        modelBuilder.Entity<Product>()
            .HasMany(x => x.ProductCategory);

        // Service relationships
        modelBuilder.Entity<Service>()
            .HasMany(x => x.ServiceCategory);

        // Sale relationships and configuration
        modelBuilder.Entity<Sale>()
            .Property(e => e.TotalAmount)
            .HasConversion<double>();
        modelBuilder.Entity<Sale>()
            .Property(e => e.TotalAmount)
            .HasPrecision(19, 4);
        modelBuilder.Entity<Sale>()
            .Property(e => e.UnitPrice)
            .HasPrecision(19, 4);
        modelBuilder.Entity<Sale>()
            .Property(e => e.DiscountAmount)
            .HasPrecision(19, 4);
        modelBuilder.Entity<Sale>()
            .Property(e => e.TaxAmount)
            .HasPrecision(19, 4);
        modelBuilder.Entity<Sale>()
            .HasOne(x => x.Product)
            .WithMany()
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<Sale>()
            .HasOne(x => x.Service)
            .WithMany()
            .HasForeignKey(x => x.ServiceId)
            .OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<Sale>()
            .HasOne(x => x.Customer)
            .WithMany()
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<Sale>()
            .HasOne(x => x.Opportunity)
            .WithMany()
            .HasForeignKey(x => x.OpportunityId)
            .OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<Sale>()
            .HasIndex(x => x.SaleDate);

        // Vendor relationships
        modelBuilder.Entity<Vendor>()
            .HasOne(x => x.Address);
        modelBuilder.Entity<Vendor>()
            .HasMany(x => x.Product);
        modelBuilder.Entity<Vendor>()
            .HasMany(x => x.Service);

        // SupportCase relationships
        modelBuilder.Entity<SupportCase>()
            .HasOne(x => x.Customer)
            .WithMany()
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<SupportCase>()
            .HasOne(x => x.Contact)
            .WithMany()
            .HasForeignKey(x => x.ContactId)
            .OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<SupportCase>()
            .HasOne(x => x.Product)
            .WithMany()
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<SupportCase>()
            .HasOne(x => x.Service)
            .WithMany()
            .HasForeignKey(x => x.ServiceId)
            .OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<SupportCase>()
            .HasIndex(x => x.Status);
        modelBuilder.Entity<SupportCase>()
            .HasIndex(x => x.Priority);

        // Reward configuration
        modelBuilder.Entity<Reward>()
            .Property(e => e.CreditsDollars)
            .HasConversion<double>();
        modelBuilder.Entity<Reward>()
            .Property(e => e.CreditsDollars)
            .HasPrecision(19, 4);
        modelBuilder.Entity<Reward>()
            .Property(e => e.ConversionRate)
            .HasConversion<double>();
        modelBuilder.Entity<Reward>()
            .Property(e => e.ConversionRate)
            .HasPrecision(19, 4);

        // Communication relationships
        modelBuilder.Entity<Communication>()
            .HasOne(x => x.Contact)
            .WithMany()
            .HasForeignKey(x => x.ContactId)
            .OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<Communication>()
            .HasOne(x => x.Lead)
            .WithMany()
            .HasForeignKey(x => x.LeadId)
            .OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<Communication>()
            .HasOne(x => x.Customer)
            .WithMany()
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<Communication>()
            .HasOne(x => x.Opportunity)
            .WithMany()
            .HasForeignKey(x => x.OpportunityId)
            .OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<Communication>()
            .HasOne(x => x.SupportCase)
            .WithMany()
            .HasForeignKey(x => x.SupportCaseId)
            .OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<Communication>()
            .HasIndex(x => x.CommunicationDate);
        modelBuilder.Entity<Communication>()
            .HasIndex(x => x.Type);

        // ActivityLog configuration
        modelBuilder.Entity<ActivityLog>()
            .HasIndex(x => x.EntityType);
        modelBuilder.Entity<ActivityLog>()
            .HasIndex(x => x.EntityId);
        modelBuilder.Entity<ActivityLog>()
            .HasIndex(x => x.Timestamp);
        modelBuilder.Entity<ActivityLog>()
            .HasIndex(x => x.UserId);

        // AuditLog configuration
        modelBuilder.Entity<AuditLog>()
            .HasIndex(x => x.EntityType);
        modelBuilder.Entity<AuditLog>()
            .HasIndex(x => x.EntityId);
        modelBuilder.Entity<AuditLog>()
            .HasIndex(x => x.Timestamp);
        modelBuilder.Entity<AuditLog>()
            .HasIndex(x => x.UserId);
        modelBuilder.Entity<AuditLog>()
            .HasIndex(x => x.CorrelationId);
    }
}

/// <summary>
/// Helper class for building audit log entries
/// </summary>
public class AuditEntry
{
    public AuditEntry(EntityEntry entry)
    {
        Entry = entry;
    }

    public EntityEntry Entry { get; }
    public string? UserId { get; set; }
    public string? UserName { get; set; }
    public string? TableName { get; set; }
    public string ChangeType { get; set; } = string.Empty;
    public string? CorrelationId { get; set; }
    public Dictionary<string, object?> KeyValues { get; } = [];
    public Dictionary<string, object?> OldValues { get; } = [];
    public Dictionary<string, object?> NewValues { get; } = [];
    public List<string> ChangedProperties { get; } = [];
    public List<PropertyEntry> TemporaryProperties { get; } = [];

    public bool HasTemporaryProperties => TemporaryProperties.Count > 0;

    public AuditLog ToAuditLog()
    {
        var jsonOptions = new JsonSerializerOptions { WriteIndented = false };

        return new AuditLog
        {
            UserId = UserId,
            UserName = UserName,
            EntityType = TableName ?? Entry.Entity.GetType().Name,
            EntityId = JsonSerializer.Serialize(KeyValues, jsonOptions),
            ChangeType = ChangeType,
            TableName = TableName,
            OldValues = OldValues.Count == 0 ? null : JsonSerializer.Serialize(OldValues, jsonOptions),
            NewValues = NewValues.Count == 0 ? null : JsonSerializer.Serialize(NewValues, jsonOptions),
            ChangedProperties = ChangedProperties.Count == 0 ? null : JsonSerializer.Serialize(ChangedProperties, jsonOptions),
            Timestamp = DateTime.UtcNow,
            CorrelationId = CorrelationId
        };
    }
}
