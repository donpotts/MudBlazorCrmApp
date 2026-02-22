using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace MudBlazorCrmApp.Shared.Models;

[DataContract]
public class SupportCase : IAuditableEntity, ISoftDelete
{
    [Key]
    [DataMember]
    public long? Id { get; set; }

    [DataMember]
    [StringLength(200)]
    public string? Title { get; set; }

    [DataMember]
    public long? CustomerId { get; set; }

    [DataMember]
    public long? ContactId { get; set; }

    [DataMember]
    public long? ProductId { get; set; }

    [DataMember]
    public long? ServiceId { get; set; }

    [DataMember]
    [StringLength(50)]
    public string? Priority { get; set; }

    [DataMember]
    [StringLength(50)]
    public string? Status { get; set; }

    [DataMember]
    [StringLength(100)]
    public string? Category { get; set; }

    [DataMember]
    [StringLength(50)]
    public string? Channel { get; set; }

    [DataMember]
    [StringLength(4000)]
    public string? Description { get; set; }

    [DataMember]
    [StringLength(4000)]
    public string? Resolution { get; set; }

    [DataMember]
    [StringLength(450)]
    public string? AssignedUserId { get; set; }

    [DataMember]
    [Range(0, 720, ErrorMessage = "SLA hours must be between 0 and 720")]
    public int? SlaHours { get; set; }

    [DataMember]
    public DateTime? DueDate { get; set; }

    [DataMember]
    public DateTime? ResolvedDate { get; set; }

    [DataMember]
    public DateTime? FollowupDate { get; set; }

    [DataMember]
    public DateTime? EscalatedDate { get; set; }

    [DataMember]
    public string? Icon { get; set; }

    [DataMember]
    [StringLength(2000)]
    public string? Notes { get; set; }

    [DataMember]
    public Customer? Customer { get; set; }

    [DataMember]
    public Contact? Contact { get; set; }

    [DataMember]
    public Product? Product { get; set; }

    [DataMember]
    public Service? Service { get; set; }

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
