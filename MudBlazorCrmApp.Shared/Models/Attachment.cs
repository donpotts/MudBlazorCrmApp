using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace MudBlazorCrmApp.Shared.Models;

[DataContract]
public class Attachment
{
    [Key]
    [DataMember]
    public long? Id { get; set; }

    [DataMember]
    [Required]
    [StringLength(255)]
    public string? FileName { get; set; }

    [DataMember]
    [StringLength(100)]
    public string? ContentType { get; set; }

    [DataMember]
    public long FileSize { get; set; }

    [DataMember]
    [StringLength(500)]
    public string? FilePath { get; set; }

    [DataMember]
    [StringLength(50)]
    public string? EntityType { get; set; }

    [DataMember]
    public long? EntityId { get; set; }

    [DataMember]
    public DateTime UploadedDate { get; set; }

    [DataMember]
    [StringLength(450)]
    public string? UploadedBy { get; set; }

    [DataMember]
    [StringLength(500)]
    public string? Description { get; set; }
}
