using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace AbsenDulu.BE.Models.LogError;
[Table("log_error_absendulu")]
public class LogErrorAbsenDulu
{
    [Column("id")]
    public Guid Id {get;set;}
    [Column("severity")]
    public string Severity {get;set;}
    [Column("error_message")]
    public string ErrorMessage {get;set;}
    [Column("status_code")]
    public string StatusCode {get;set;}
    [Column("method")]
    public string Method {get;set;}
    [Column("payload")]
    public string Payload {get;set;}
    [Column("service")]
    public string Service {get;set;}
    [Column("ip_address")]
    public string IpAddress {get;set;}
    [Column("client_name")]
    public string ClientName {get;set;}
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    [Column("created_by")]
    public string CreatedBy {get;set;}

}