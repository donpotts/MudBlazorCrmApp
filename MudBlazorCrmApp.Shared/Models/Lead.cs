using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace MudBlazorCrmApp.Shared.Models;

[DataContract]
public class Lead
{
    [Key]
    [DataMember]
    public long? Id { get; set; }

    [DataMember]
    public long? ContactId { get; set; }

    [DataMember]
    public string? Source { get; set; }

    [DataMember]
    public string? Status { get; set; }

    [DataMember]
    public decimal? PotentialValue { get; set; }

    [DataMember]
    public long? OpportunityId { get; set; }

    [DataMember]
    public long? AddressId { get; set; }

    [DataMember]
    public string? Photo { get; set; }

    [DataMember]
    public string? Notes { get; set; }

    [DataMember]
    public List<Address>? Address { get; set; }

    [DataMember]
    public List<Opportunity>? Opportunity { get; set; }

    [DataMember]
    public List<Contact>? Contact { get; set; }
}
