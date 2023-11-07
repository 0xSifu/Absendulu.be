using AbsenDulu.BE.DTO.Company;
using AbsenDulu.BE.Models.Company;
namespace AbsenDulu.BE.Interfaces.IServices;
public interface IWorkshopService
{
    List<MasterWorkshop> GetWorkshop(Guid request);
    MasterWorkshop AddWorkshop(MasterWorkshopDTO request);
    bool RemoveWorkshop(string Id);
    MasterWorkshop UpdateWorkshop(MasterWorkshopDTO request);
    List<MasterWorkshop> FindByContains(string request , Guid companyId);
}