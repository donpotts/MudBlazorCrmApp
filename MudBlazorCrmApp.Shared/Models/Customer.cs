using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace MudBlazorCrmApp.Shared.Models;

[DataContract]
public class Customer
{
    [Key]
    [DataMember]
    public long? Id { get; set; }

    [DataMember]
    public string? Name { get; set; }

    [DataMember]
    public string? Type { get; set; }

    [DataMember]
    public string? Industry { get; set; }

    [DataMember]
    public long? AddressId { get; set; }

    [DataMember]
    public long? ContactId { get; set; }

    [DataMember]
    public string? Logo { get; set; }

    [DataMember]
    public string? Notes { get; set; }

    [DataMember]
    public List<Address>? Address { get; set; }

    [DataMember]
    public List<Contact>? Contact { get; set; }
}
