using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace MudBlazorCrmApp.Shared.Models;

[DataContract]
public class EntityTag
{
    [Key]
    [DataMember]
    public long? Id { get; set; }

    [DataMember]
    public long? TagId { get; set; }

    [DataMember]
    public string? EntityType { get; set; }

    [DataMember]
    public long? EntityId { get; set; }

    [DataMember]
    public Tag? Tag { get; set; }

    [DataMember]
    public DateTime? CreatedDate { get; set; }

    [DataMember]
    public DateTime ModifiedDate { get; set; }
}
