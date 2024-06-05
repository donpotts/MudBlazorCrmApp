using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace MudBlazorCrmApp.Shared.Models;

[DataContract]
public class Reward
{
    [Key]
    [DataMember]
    public long? Id { get; set; }

    [DataMember]
    public long? Rewardpoints { get; set; }

    [DataMember]
    public decimal? CreditsDollars { get; set; }

    [DataMember]
    public decimal? ConversionRate { get; set; }

    [DataMember]
    public DateTime? ExpirationDate { get; set; }

    [DataMember]
    public string? Notes { get; set; }
}
