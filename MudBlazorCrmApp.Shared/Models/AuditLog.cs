using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace MudBlazorCrmApp.Shared.Models;

/// <summary>
/// Detailed audit log for tracking all entity changes with field-level granularity
/// </summary>
[DataContract]
public class AuditLog
{
    [Key]
    [DataMember]
    public long Id { get; set; }

    /// <summary>
    /// The user ID who made the change
    /// </summary>
    [DataMember]
    [StringLength(450)]
    public string? UserId { get; set; }

    /// <summary>
    /// Display name of the user for quick reference
    /// </summary>
    [DataMember]
    [StringLength(200)]
    public string? UserName { get; set; }

    /// <summary>
    /// Type of entity that was changed (Customer, Lead, Sale, etc.)
    /// </summary>
    [DataMember]
    [Required]
    [StringLength(100)]
    public string EntityType { get; set; } = string.Empty;

    /// <summary>
    /// Primary key of the entity that was changed
    /// </summary>
    [DataMember]
    [Required]
    [StringLength(100)]
    public string EntityId { get; set; } = string.Empty;

    /// <summary>
    /// Type of change: Insert, Update, Delete, SoftDelete, Restore
    /// </summary>
    [DataMember]
    [Required]
    [StringLength(50)]
    public string ChangeType { get; set; } = string.Empty;

    /// <summary>
    /// Name of the table/entity in the database
    /// </summary>
    [DataMember]
    [StringLength(100)]
    public string? TableName { get; set; }

    /// <summary>
    /// JSON representation of the entity's old values (before change)
    /// </summary>
    [DataMember]
    public string? OldValues { get; set; }

    /// <summary>
    /// JSON representation of the entity's new values (after change)
    /// </summary>
    [DataMember]
    public string? NewValues { get; set; }

    /// <summary>
    /// JSON array of property names that were changed
    /// </summary>
    [DataMember]
    public string? ChangedProperties { get; set; }

    /// <summary>
    /// IP address of the user who made the change
    /// </summary>
    [DataMember]
    [StringLength(50)]
    public string? IpAddress { get; set; }

    /// <summary>
    /// User agent/browser information
    /// </summary>
    [DataMember]
    [StringLength(500)]
    public string? UserAgent { get; set; }

    /// <summary>
    /// Additional context or notes about the change
    /// </summary>
    [DataMember]
    [StringLength(1000)]
    public string? AdditionalInfo { get; set; }

    /// <summary>
    /// Timestamp when the change occurred
    /// </summary>
    [DataMember]
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Correlation ID to group related changes in a single transaction
    /// </summary>
    [DataMember]
    [StringLength(100)]
    public string? CorrelationId { get; set; }
}

/// <summary>
/// Constants for audit log change types
/// </summary>
public static class AuditChangeTypes
{
    public const string Insert = "Insert";
    public const string Update = "Update";
    public const string Delete = "Delete";
    public const string SoftDelete = "SoftDelete";
    public const string Restore = "Restore";
    public const string Login = "Login";
    public const string Logout = "Logout";
}
