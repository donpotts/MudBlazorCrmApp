using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace MudBlazorCrmApp.Shared.Models;

[DataContract]
public class Sale : IAuditableEntity, ISoftDelete
{
    [Key]
    [DataMember]
    public long? Id { get; set; }

    [DataMember]
    public long? ProductId { get; set; }

    [DataMember]
    public long? ServiceId { get; set; }

    [DataMember]
    public long? CustomerId { get; set; }

    [DataMember]
    public long? OpportunityId { get; set; }

    [DataMember]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
    public long? Quantity { get; set; }

    [DataMember]
    [Range(0, double.MaxValue, ErrorMessage = "Unit price must be positive")]
    public decimal? UnitPrice { get; set; }

    [DataMember]
    [Range(0, double.MaxValue, ErrorMessage = "Discount must be positive")]
    public decimal? DiscountAmount { get; set; }

    [DataMember]
    [Range(0, double.MaxValue, ErrorMessage = "Tax must be positive")]
    public decimal? TaxAmount { get; set; }

    [DataMember]
    [Range(0, double.MaxValue, ErrorMessage = "Total amount must be positive")]
    public decimal? TotalAmount { get; set; }

    [DataMember]
    public DateTime? SaleDate { get; set; }

    [DataMember]
    [StringLength(50)]
    public string? Status { get; set; }

    [DataMember]
    [StringLength(50)]
    public string? PaymentMethod { get; set; }

    [DataMember]
    [StringLength(50)]
    public string? PaymentStatus { get; set; }

    [DataMember]
    [StringLength(100)]
    public string? InvoiceNumber { get; set; }

    [DataMember]
    [StringLength(450)]
    public string? SalesRepId { get; set; }

    [DataMember]
    public string? ReceiptPhoto { get; set; }

    [DataMember]
    [StringLength(2000)]
    public string? Notes { get; set; }

    [DataMember]
    public Product? Product { get; set; }

    [DataMember]
    public Service? Service { get; set; }

    [DataMember]
    public Customer? Customer { get; set; }

    [DataMember]
    public Opportunity? Opportunity { get; set; }

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
