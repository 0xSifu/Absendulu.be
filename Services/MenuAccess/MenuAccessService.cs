using AbsenDulu.BE.Database.Helper.Context;
using AbsenDulu.BE.Interfaces.IServices.MenuAccess;
using AbsenDulu.BE.Models.MenuAccess;
using Newtonsoft.Json;

namespace AbsenDulu.BE.Services.MenuAccess;
public class MenuAccessService: IMenuAccessService
{
    private readonly DataContext _context;
    public MenuAccessService(DataContext dataContext)
    {
        _context=dataContext;
    }

    public List<AccessMenu> GetMenuAccess(Guid companyId,Guid positionId)
    {
        try
        {
            var data = _context.menu_access.Where(d=> d.CompanyId==companyId && d.PositionId==positionId).ToList();
            if (data == null || data.Count < 1)
            {
                throw new Exception("Data Access Menu Not Found");
            }
            // data. = JsonConvert.DeserializeObject<AccessMenu>(json);
            return data;
        }
        catch
        {
            throw;
        }
    }
}