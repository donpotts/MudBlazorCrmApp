using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace MudBlazorCrmApp.Shared.Models;

[DataContract]
public class Contact
{
    [Key]
    [DataMember]
    public long? Id { get; set; }

    [DataMember]
    public string? Name { get; set; }

    [DataMember]
    public string? Email { get; set; }

    [DataMember]
    public long? Phone { get; set; }

    [DataMember]
    public string? Role { get; set; }

    [DataMember]
    public long? AddressId { get; set; }

    [DataMember]
    public string? Photo { get; set; }

    [DataMember]
    public long? RewardId { get; set; }

    [DataMember]
    public string? Notes { get; set; }

    [DataMember]
    public List<Address>? Address { get; set; }

    [DataMember]
    public List<Reward>? Reward { get; set; }
}
