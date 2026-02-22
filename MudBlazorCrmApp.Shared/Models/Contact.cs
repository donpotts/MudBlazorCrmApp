using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace MudBlazorCrmApp.Shared.Models;

[DataContract]
public class Contact : IAuditableEntity, ISoftDelete
{
    [Key]
    [DataMember]
    public long? Id { get; set; }

    [DataMember]
    [Required(ErrorMessage = "Contact name is required")]
    [StringLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
    public string? Name { get; set; }

    [DataMember]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [StringLength(255)]
    public string? Email { get; set; }

    [DataMember]
    [Phone(ErrorMessage = "Invalid phone number format")]
    [StringLength(50)]
    public string? Phone { get; set; }

    [DataMember]
    [StringLength(50)]
    public string? MobilePhone { get; set; }

    [DataMember]
    [StringLength(200)]
    public string? Company { get; set; }

    [DataMember]
    [StringLength(100)]
    public string? Title { get; set; }

    [DataMember]
    [StringLength(100)]
    public string? Role { get; set; }

    [DataMember]
    [StringLength(100)]
    public string? Department { get; set; }

    [DataMember]
    [StringLength(50)]
    public string? PreferredContactMethod { get; set; }

    [DataMember]
    [StringLength(50)]
    public string? Status { get; set; }

    [DataMember]
    public long? CustomerId { get; set; }

    [DataMember]
    public long? AddressId { get; set; }

    [DataMember]
    [StringLength(255)]
    public string? LinkedInUrl { get; set; }

    [DataMember]
    public bool IsPrimary { get; set; }

    [DataMember]
    public bool DoNotContact { get; set; }

    [DataMember]
    public string? Photo { get; set; }

    [DataMember]
    public long? RewardId { get; set; }

    [DataMember]
    [StringLength(2000)]
    public string? Notes { get; set; }

    [DataMember]
    public Address? Address { get; set; }

    [DataMember]
    public Customer? Customer { get; set; }

    [DataMember]
    public List<Reward>? Reward { get; set; }

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
