using AbsenDulu.BE.Database.Helper.Context;
using AbsenDulu.BE.Interfaces.IServices.MenuAccess;
using AbsenDulu.BE.Models.MenuAccess;

namespace AbsenDulu.BE.Services.MenuAccess;
public class AvailableAccessService: IAvailableAccessService
{
    private readonly DataContext _context;
    public AvailableAccessService(DataContext dataContext)
    {
        _context=dataContext;
    }
    public List<AvailablePackage> GetAvailableAccess(Guid companyId)
    {
        try
        {
            var subscribedPackage = _context.details_subcribe.Where(x=> x.CompanyId==companyId).ToList();
            var data = _context.available_menu.Where(d => d.Id == subscribedPackage.FirstOrDefault().SubscribedPackage).ToList();
            if (data == null || data.Count < 1)
            {
                throw new Exception("Data Access Menu Not Found");
            }
            return data;
        }
        catch
        {
            throw;
        }
    }
}