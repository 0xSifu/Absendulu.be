using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace AbsenDulu.BE.Models.MenuAccess;
[Table("menu_access")]
public class AccessMenu
{
    [Column("id")]
    public Guid Id { get; set; }
    [Column("company_id")]
    public Guid CompanyId { get; set; }
    [Column("position_id")]
    public Guid PositionId { get; set; }
    [JsonProperty("access")]
    [Column(TypeName = "json")]
    public List<JsonContentMenu>? access { get; set; }
    [Column("updated_at")]
    public DateTime? UpdatedDate { get; set; }
}