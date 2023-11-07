using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace AbsenDulu.BE.Models.MenuAccess;
[Table("available_packages")]
public class AvailablePackage
{
    [Column("id")]
    public Guid Id { get; set; }
    [Column("package_name")]
    public string PackageName { get; set; }
    [JsonProperty("available_menu")]
    [Column(TypeName = "json")]
    public List<JsonContentMenu>? available_menu { get; set; }
    [Column("available_device")]
    public int AvailableDevice { get; set; }
    [Column("available_apps")]
    public int AvailableApps { get; set; }
}