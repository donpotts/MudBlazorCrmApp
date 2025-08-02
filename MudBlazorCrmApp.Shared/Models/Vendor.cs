using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace MudBlazorCrmApp.Shared.Models;

[DataContract]
public class Vendor
{
    [Key]
    [DataMember]
    public long? Id { get; set; }

    [DataMember]
    public string? Name { get; set; }

    [DataMember]
    public string? ContactName { get; set; }

    [DataMember]
    public long? Phone { get; set; }

    [DataMember]
    public string? Email { get; set; }

    [DataMember]
    public long? AddressId { get; set; }

    [DataMember]
    public long? ProductId { get; set; }

    [DataMember]
    public long? ServiceId { get; set; }

    [DataMember]
    public string? Logo { get; set; }

    [DataMember]
    public string? Notes { get; set; }

    [DataMember]
    public List<Address>? Address { get; set; }

    [DataMember]
    public List<Product>? Product { get; set; }

    [DataMember]
    public List<Service>? Service { get; set; }
}
