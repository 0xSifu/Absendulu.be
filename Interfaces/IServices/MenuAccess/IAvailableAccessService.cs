using AbsenDulu.BE.Models.MenuAccess;

namespace AbsenDulu.BE.Interfaces.IServices.MenuAccess;
public interface IAvailableAccessService
{
    List<AvailablePackage> GetAvailableAccess(Guid companyId);
}