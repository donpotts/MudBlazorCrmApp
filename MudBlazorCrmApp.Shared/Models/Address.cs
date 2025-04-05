using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace MudBlazorCrmApp.Shared.Models;

[DataContract]
public class Address
{
    [Key]
    [DataMember]
    public long? Id { get; set; }

    [DataMember]
    public string? Street { get; set; }

    [DataMember]
    public string? City { get; set; }

    [DataMember]
    public string? State { get; set; }

    [DataMember]
    public long? ZipCode { get; set; }

    [DataMember]
    public string? Country { get; set; }

    [DataMember]
    public string? Photo { get; set; }

    [DataMember]
    public string? Notes { get; set; }
    
    [DataMember]
    public DateTime? CreatedDate { get; set; }

    [DataMember]
    public DateTime ModifiedDate { get; set; }

}
