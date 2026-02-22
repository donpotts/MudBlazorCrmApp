using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace MudBlazorCrmApp.Shared.Models;

[DataContract]
public class Customer : IAuditableEntity, ISoftDelete
{
    [Key]
    [DataMember]
    public long? Id { get; set; }

    [DataMember]
    [Required(ErrorMessage = "Customer name is required")]
    [StringLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
    public string? Name { get; set; }

    [DataMember]
    [StringLength(50)]
    public string? Type { get; set; }

    [DataMember]
    [StringLength(100)]
    public string? Industry { get; set; }

    [DataMember]
    [StringLength(100)]
    public string? Website { get; set; }

    [DataMember]
    [Range(0, int.MaxValue, ErrorMessage = "Employee count must be positive")]
    public int? EmployeeCount { get; set; }

    [DataMember]
    [Range(0, double.MaxValue, ErrorMessage = "Annual revenue must be positive")]
    public decimal? AnnualRevenue { get; set; }

    [DataMember]
    public long? AddressId { get; set; }

    [DataMember]
    public long? ContactId { get; set; }

    [DataMember]
    [StringLength(450)]
    public string? AccountManagerId { get; set; }

    [DataMember]
    [StringLength(50)]
    public string? Status { get; set; }

    [DataMember]
    public string? Logo { get; set; }

    [DataMember]
    [StringLength(2000)]
    public string? Notes { get; set; }

    [DataMember]
    public Address? Address { get; set; }

    [DataMember]
    public List<Contact>? Contact { get; set; }

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
