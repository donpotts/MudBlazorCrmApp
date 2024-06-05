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
    public DateTime? EstimatedCloseDate { get; set; }

    [DataMember]
    public string? Stage { get; set; }

    [DataMember]
    public string? Icon { get; set; }

    [DataMember]
    public string? Notes { get; set; }
}
