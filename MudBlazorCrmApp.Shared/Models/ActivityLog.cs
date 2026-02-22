using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace MudBlazorCrmApp.Shared.Models;

/// <summary>
/// Tracks all user actions for audit trail and activity feed
/// </summary>
[DataContract]
public class ActivityLog
{
    [Key]
    [DataMember]
    public long Id { get; set; }

    /// <summary>
    /// The user who performed the action
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
    /// Type of entity affected (Customer, Lead, Sale, Opportunity, etc.)
    /// </summary>
    [DataMember]
    [Required]
    [StringLength(100)]
    public string? EntityType { get; set; }

    /// <summary>
    /// ID of the affected entity
    /// </summary>
    [DataMember]
    public long? EntityId { get; set; }

    /// <summary>
    /// Name/title of the entity for quick reference without joining
    /// </summary>
    [DataMember]
    [StringLength(200)]
    public string? EntityName { get; set; }

    /// <summary>
    /// Type of action performed (Created, Updated, Deleted, Viewed, StatusChanged, Assigned, etc.)
    /// </summary>
    [DataMember]
    [Required]
    [StringLength(50)]
    public string? Action { get; set; }

    /// <summary>
    /// Human-readable description of the action
    /// </summary>
    [DataMember]
    [StringLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// JSON representation of old values (for updates)
    /// </summary>
    [DataMember]
    public string? OldValues { get; set; }

    /// <summary>
    /// JSON representation of new values (for creates and updates)
    /// </summary>
    [DataMember]
    public string? NewValues { get; set; }

    /// <summary>
    /// Specific field that was changed (for single-field updates)
    /// </summary>
    [DataMember]
    [StringLength(100)]
    public string? ChangedField { get; set; }

    /// <summary>
    /// IP address of the user
    /// </summary>
    [DataMember]
    [StringLength(50)]
    public string? IpAddress { get; set; }

    /// <summary>
    /// User agent/browser info
    /// </summary>
    [DataMember]
    [StringLength(500)]
    public string? UserAgent { get; set; }

    /// <summary>
    /// When the action occurred
    /// </summary>
    [DataMember]
    public DateTime Timestamp { get; set; }
}

/// <summary>
/// Constants for activity log actions
/// </summary>
public static class ActivityActions
{
    public const string Created = "Created";
    public const string Updated = "Updated";
    public const string Deleted = "Deleted";
    public const string Viewed = "Viewed";
    public const string StatusChanged = "StatusChanged";
    public const string Assigned = "Assigned";
    public const string Converted = "Converted";
    public const string Exported = "Exported";
    public const string Imported = "Imported";
    public const string Login = "Login";
    public const string Logout = "Logout";
}

/// <summary>
/// Constants for entity types in activity logs
/// </summary>
public static class EntityTypes
{
    public const string Customer = "Customer";
    public const string Contact = "Contact";
    public const string Lead = "Lead";
    public const string Opportunity = "Opportunity";
    public const string Sale = "Sale";
    public const string Product = "Product";
    public const string Service = "Service";
    public const string SupportCase = "SupportCase";
    public const string TodoTask = "TodoTask";
    public const string User = "User";
}
