using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace MudBlazorCrmApp.Shared.Models;

[DataContract]
public class SupportCase
{
    [Key]
    [DataMember]
    public long? Id { get; set; }

    [DataMember]
    public long? CustomerId { get; set; }

    [DataMember]
    public long? ProductId { get; set; }

    [DataMember]
    public string? ServiceId { get; set; }

    [DataMember]
    public string? Status { get; set; }

    [DataMember]
    public string? Description { get; set; }

    [DataMember]
    public DateTime? CreatedDateTime { get; set; }

    [DataMember]
    public DateTime? ModifiedDateTime { get; set; }

    [DataMember]
    public long? UserId { get; set; }

    [DataMember]
    public string? FollowupDate { get; set; }

    [DataMember]
    public string? Icon { get; set; }

    [DataMember]
    public string? Notes { get; set; }
}
