namespace AbsenDulu.BE.DTO.LogError;
public class logAbsenDuluDTO
{
    public string? Severity { get; set; }
    public string? ErrorMessage { get; set; }
    public string? StatusCode { get; set; }
    public string? Method { get; set; }
    public string? Payload { get; set; }
    public string? Service { get; set; }
    public string? IpAddress { get; set; }
    public string? ClientName { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? CreatedAt { get; set; }

}