using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace MudBlazorCrmApp.Shared.Models;

[DataContract]
public class Notification
{
    [Key]
    [DataMember]
    public long? Id { get; set; }

    [DataMember]
    [Required]
    [StringLength(200)]
    public string? Title { get; set; }

    [DataMember]
    [StringLength(500)]
    public string? Message { get; set; }

    [DataMember]
    [StringLength(50)]
    public string? Type { get; set; } // Info, Warning, Alert, Reminder

    [DataMember]
    [StringLength(50)]
    public string? Priority { get; set; } // Low, Normal, High, Urgent

    [DataMember]
    public bool IsRead { get; set; }

    [DataMember]
    public DateTime CreatedDate { get; set; }

    [DataMember]
    public DateTime? ReadDate { get; set; }

    [DataMember]
    [StringLength(450)]
    public string? UserId { get; set; }

    [DataMember]
    [StringLength(50)]
    public string? EntityType { get; set; }

    [DataMember]
    public long? EntityId { get; set; }

    [DataMember]
    [StringLength(200)]
    public string? ActionUrl { get; set; }
}
