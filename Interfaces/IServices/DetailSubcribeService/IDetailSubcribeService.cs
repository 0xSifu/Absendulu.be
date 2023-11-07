using AbsenDulu.BE.Models.Subcribes;

namespace AbsenDulu.BE.Interfaces.IServices.DetailSubcribeService;
public interface IDetailSubcribeService
{
    List<DetailSubcribe> GetDetailsSubcribes(Guid companyId);
    DetailSubcribe UpdateAppsUsed(string id);
    DetailSubcribe UpdateDeviceUsed(string id);
}