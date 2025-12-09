using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace MudBlazorCrmApp.Shared.Models;

[DataContract]
public class Opportunity : IAuditableEntity, ISoftDelete
{
  [Key]
  [DataMember]
  public long? Id { get; set; }

  [DataMember]
  [Required(ErrorMessage = "Opportunity name is required")]
  [StringLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
  public string? Name { get; set; }

  [DataMember]
  [Range(0, double.MaxValue, ErrorMessage = "Value must be a positive number")]
  public decimal? Value { get; set; }

  [DataMember]
  [Range(0, 100, ErrorMessage = "Probability must be between 0 and 100")]
  public int? Probability { get; set; }

  [DataMember]
  public DateTime? EstimatedCloseDate { get; set; }

  [DataMember]
  public DateTime? ActualCloseDate { get; set; }

  [DataMember]
  [StringLength(50)]
  public string? Stage { get; set; }

  [DataMember]
  public long? CustomerId { get; set; }

  [DataMember]
  public long? ContactId { get; set; }

  [DataMember]
  [StringLength(450)]
  public string? AssignedUserId { get; set; }

  [DataMember]
  [StringLength(500)]
  public string? LostReason { get; set; }

  [DataMember]
  [StringLength(1000)]
  public string? CompetitorInfo { get; set; }

  [DataMember]
  public string? Icon { get; set; }

  [DataMember]
  [StringLength(2000)]
  public string? Notes { get; set; }

  [DataMember]
  public Customer? Customer { get; set; }

  [DataMember]
  public Contact? Contact { get; set; }

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
