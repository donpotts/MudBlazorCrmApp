using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace MudBlazorCrmApp.Shared.Models;

/// <summary>
/// Tracks all communications and interactions with contacts, leads, and customers
/// </summary>
[DataContract]
public class Communication : IAuditableEntity, ISoftDelete
{
    [Key]
    [DataMember]
    public long Id { get; set; }

    /// <summary>
    /// Type of communication (Email, Call, Meeting, Note, SMS, etc.)
    /// </summary>
    [DataMember]
    [Required]
    [StringLength(50)]
    public string? Type { get; set; }

    /// <summary>
    /// Direction of communication (Inbound, Outbound)
    /// </summary>
    [DataMember]
    [StringLength(20)]
    public string? Direction { get; set; }

    /// <summary>
    /// Subject line or brief title
    /// </summary>
    [DataMember]
    [StringLength(500)]
    public string? Subject { get; set; }

    /// <summary>
    /// Full content of the communication
    /// </summary>
    [DataMember]
    public string? Body { get; set; }

    /// <summary>
    /// Status of the communication (Completed, Pending, Scheduled, Cancelled)
    /// </summary>
    [DataMember]
    [StringLength(50)]
    public string? Status { get; set; }

    /// <summary>
    /// Related contact
    /// </summary>
    [DataMember]
    public long? ContactId { get; set; }

    /// <summary>
    /// Related lead
    /// </summary>
    [DataMember]
    public long? LeadId { get; set; }

    /// <summary>
    /// Related customer
    /// </summary>
    [DataMember]
    public long? CustomerId { get; set; }

    /// <summary>
    /// Related opportunity
    /// </summary>
    [DataMember]
    public long? OpportunityId { get; set; }

    /// <summary>
    /// Related support case
    /// </summary>
    [DataMember]
    public long? SupportCaseId { get; set; }

    /// <summary>
    /// User who created/logged the communication
    /// </summary>
    [DataMember]
    [StringLength(450)]
    public string? UserId { get; set; }

    /// <summary>
    /// When the communication occurred or is scheduled
    /// </summary>
    [DataMember]
    public DateTime? CommunicationDate { get; set; }

    /// <summary>
    /// Duration in minutes (for calls and meetings)
    /// </summary>
    [DataMember]
    [Range(0, 1440, ErrorMessage = "Duration must be between 0 and 1440 minutes")]
    public int? DurationMinutes { get; set; }

    /// <summary>
    /// Outcome or result of the communication
    /// </summary>
    [DataMember]
    [StringLength(500)]
    public string? Outcome { get; set; }

    /// <summary>
    /// Follow-up required flag
    /// </summary>
    [DataMember]
    public bool FollowUpRequired { get; set; }

    /// <summary>
    /// Follow-up date if required
    /// </summary>
    [DataMember]
    public DateTime? FollowUpDate { get; set; }

    /// <summary>
    /// Additional notes
    /// </summary>
    [DataMember]
    [StringLength(2000)]
    public string? Notes { get; set; }

    // Navigation properties
    [DataMember]
    public Contact? Contact { get; set; }

    [DataMember]
    public Lead? Lead { get; set; }

    [DataMember]
    public Customer? Customer { get; set; }

    [DataMember]
    public Opportunity? Opportunity { get; set; }

    [DataMember]
    public SupportCase? SupportCase { get; set; }

    // IAuditableEntity implementation
    [DataMember]
    public DateTime? CreatedDate { get; set; }

    [DataMember]
    [StringLength(450)]
    public string? CreatedBy { get; set; }

    [DataMember]
    public DateTime ModifiedDate { get; set; }

    [DataMember]
    [StringLength(450)]
    public string? ModifiedBy { get; set; }

    // ISoftDelete implementation
    [DataMember]
    public bool IsDeleted { get; set; }

    [DataMember]
    public DateTime? DeletedDate { get; set; }

    [DataMember]
    [StringLength(450)]
    public string? DeletedBy { get; set; }
}

/// <summary>
/// Constants for communication types
/// </summary>
public static class CommunicationTypes
{
    public const string Email = "Email";
    public const string Call = "Call";
    public const string Meeting = "Meeting";
    public const string Note = "Note";
    public const string SMS = "SMS";
    public const string VideoCall = "VideoCall";
    public const string InPerson = "InPerson";
    public const string SocialMedia = "SocialMedia";
}

/// <summary>
/// Constants for communication directions
/// </summary>
public static class CommunicationDirections
{
    public const string Inbound = "Inbound";
    public const string Outbound = "Outbound";
}
