using AbsenDulu.BE.Models.MenuAccess;

namespace AbsenDulu.BE.Interfaces.IServices.MenuAccess;
public interface IMenuAccessService
{
    List<AccessMenu> GetMenuAccess(Guid companyId, Guid positionId);
}