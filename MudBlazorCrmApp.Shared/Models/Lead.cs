using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace MudBlazorCrmApp.Shared.Models;

[DataContract]
public class Lead : IAuditableEntity, ISoftDelete
{
    [Key]
    [DataMember]
    public long? Id { get; set; }

    [DataMember]
    [StringLength(200)]
    public string? Company { get; set; }

    [DataMember]
    [StringLength(100)]
    public string? Title { get; set; }

    [DataMember]
    [StringLength(100)]
    public string? Industry { get; set; }

    [DataMember]
    public long? ContactId { get; set; }

    [DataMember]
    [StringLength(100)]
    public string? Source { get; set; }

    [DataMember]
    [StringLength(50)]
    public string? Status { get; set; }

    [DataMember]
    [Range(0, double.MaxValue, ErrorMessage = "Potential value must be positive")]
    public decimal? PotentialValue { get; set; }

    [DataMember]
    [Range(0, 100, ErrorMessage = "Lead score must be between 0 and 100")]
    public int? LeadScore { get; set; }

    [DataMember]
    [StringLength(450)]
    public string? AssignedUserId { get; set; }

    [DataMember]
    public DateTime? LastContactDate { get; set; }

    [DataMember]
    public long? OpportunityId { get; set; }

    [DataMember]
    public long? AddressId { get; set; }

    [DataMember]
    public long? ConvertedToCustomerId { get; set; }

    [DataMember]
    public DateTime? ConvertedDate { get; set; }

    [DataMember]
    [StringLength(500)]
    public string? ConversionNotes { get; set; }

    [DataMember]
    public string? Photo { get; set; }

    [DataMember]
    [StringLength(2000)]
    public string? Notes { get; set; }

    [DataMember]
    public Address? Address { get; set; }

    [DataMember]
    public Opportunity? Opportunity { get; set; }

    [DataMember]
    public Contact? Contact { get; set; }

    [DataMember]
    public Customer? ConvertedToCustomer { get; set; }

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
