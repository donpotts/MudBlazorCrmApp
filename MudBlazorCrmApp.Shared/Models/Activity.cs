using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace MudBlazorCrmApp.Shared.Models;

[DataContract]
public class Activity
{
    [Key]
    [DataMember]
    public long? Id { get; set; }

    [DataMember]
    public string? Type { get; set; }

    [DataMember]
    public string? Subject { get; set; }

    [DataMember]
    public string? Description { get; set; }

    [DataMember]
    public DateTime? ActivityDate { get; set; }

    [DataMember]
    public int? DurationMinutes { get; set; }

    [DataMember]
    public string? Status { get; set; }

    [DataMember]
    public string? Direction { get; set; }

    [DataMember]
    public long? ContactId { get; set; }

    [DataMember]
    public long? CustomerId { get; set; }

    [DataMember]
    public long? LeadId { get; set; }

    [DataMember]
    public long? OpportunityId { get; set; }

    [DataMember]
    public string? CreatedBy { get; set; }

    [DataMember]
    public Contact? Contact { get; set; }

    [DataMember]
    public Customer? Customer { get; set; }

    [DataMember]
    public Lead? Lead { get; set; }

    [DataMember]
    public Opportunity? Opportunity { get; set; }

    [DataMember]
    public DateTime? CreatedDate { get; set; }

    [DataMember]
    public DateTime ModifiedDate { get; set; }
}
