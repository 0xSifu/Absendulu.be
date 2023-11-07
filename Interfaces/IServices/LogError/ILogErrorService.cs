using AbsenDulu.BE.DTO.LogError;

namespace AbsenDulu.BE.Interfaces.IServices.LogError;
public interface ILogErrorService
{
    void AddLog(logAbsenDuluDTO log);
}