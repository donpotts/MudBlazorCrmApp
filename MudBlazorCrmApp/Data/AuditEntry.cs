using Microsoft.EntityFrameworkCore.ChangeTracking;
using MudBlazorCrmApp.Shared.Models;
using System.Text.Json;

namespace MudBlazorCrmApp.Data;

/// <summary>
/// Helper class to collect audit information during SaveChanges
/// </summary>
public class AuditEntry
{
    public AuditEntry(EntityEntry entry)
    {
        Entry = entry;
    }

    public EntityEntry Entry { get; }
    public string? TableName { get; set; }
    public string? UserId { get; set; }
    public string? UserName { get; set; }
    public string? CorrelationId { get; set; }
    public string ChangeType { get; set; } = string.Empty;
    public Dictionary<string, object?> KeyValues { get; } = new();
    public Dictionary<string, object?> OldValues { get; } = new();
    public Dictionary<string, object?> NewValues { get; } = new();
    public List<string> ChangedProperties { get; } = new();
    public List<PropertyEntry> TemporaryProperties { get; } = new();

    public bool HasTemporaryProperties => TemporaryProperties.Count > 0;

    public AuditLog ToAuditLog()
    {
        return new AuditLog
        {
            UserId = UserId,
            UserName = UserName,
            EntityType = TableName ?? string.Empty,
            EntityId = JsonSerializer.Serialize(KeyValues),
            ChangeType = ChangeType,
            TableName = TableName,
            OldValues = OldValues.Count == 0 ? null : JsonSerializer.Serialize(OldValues),
            NewValues = NewValues.Count == 0 ? null : JsonSerializer.Serialize(NewValues),
            ChangedProperties = ChangedProperties.Count == 0 ? null : JsonSerializer.Serialize(ChangedProperties),
            Timestamp = DateTime.UtcNow,
            CorrelationId = CorrelationId
        };
    }
}
