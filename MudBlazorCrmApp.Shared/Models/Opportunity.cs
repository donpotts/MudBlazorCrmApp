using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace MudBlazorCrmApp.Shared.Models;

[DataContract]
public class Opportunity
{
    [Key]
    [DataMember]
    public long? Id { get; set; }

    [DataMember]
    public string? Name { get; set; }

    [DataMember]
    public decimal? Value { get; set; }

    [DataMember]
    public double? Probability { get; set; }

    [DataMember]
    public string? Source { get; set; }

    [DataMember]
    public DateTime? EstimatedCloseDate { get; set; }

    [DataMember]
    public string? Stage { get; set; }

    [DataMember]
    public long? CustomerId { get; set; }

    [DataMember]
    public long? ContactId { get; set; }

    [DataMember]
    public string? Icon { get; set; }

    [DataMember]
    public string? Notes { get; set; }

    [DataMember]
    public Customer? Customer { get; set; }

    [DataMember]
    public Contact? Contact { get; set; }

    [DataMember]
    public DateTime? CreatedDate { get; set; }

    [DataMember]
    public DateTime ModifiedDate { get; set; }

}
