using AbsenDulu.BE.Database.Helper.Context;
using AbsenDulu.BE.DTO.LogError;
using AbsenDulu.BE.Interfaces.IServices.LogError;
using AbsenDulu.BE.Models.LogError;

namespace AbsenDulu.BE.Services.LogError;
public class LogErrorService: ILogErrorService
{
    private readonly DataContext _context;
    public LogErrorService(DataContext context)
    {
        _context=context;
    }
    public void AddLog(logAbsenDuluDTO log)
    {
        try
        {
            LogErrorAbsenDulu data = new LogErrorAbsenDulu
            {
                Id = Guid.NewGuid(),
                Severity=log.Severity,
                ErrorMessage=log.ErrorMessage,
                Method=log.Method,
                Service=log.Service,
                Payload=log.Payload,
                StatusCode=log.StatusCode,
                ClientName=log.ClientName,
                IpAddress=log.IpAddress,
                CreatedBy = log.CreatedBy,
                CreatedAt = DateTime.UtcNow
            };
            _context.log_error_absendulu.Add(data);
            _context.SaveChanges();
        }
        catch (Exception exception)
        {
            throw new Exception(exception.Message);
        }
    }
}