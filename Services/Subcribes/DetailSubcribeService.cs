using AbsenDulu.BE.Database.Helper.Context;
using AbsenDulu.BE.Interfaces.IServices.DetailSubcribeService;
using AbsenDulu.BE.Models.Subcribes;

namespace AbsenDulu.BE.Services.Subcribes;

public class DetailSubcribeService:IDetailSubcribeService
{
    private readonly DataContext _context;
    public DetailSubcribeService(DataContext dataContext )
    {
        _context=dataContext;
    }

    public List<DetailSubcribe> GetDetailsSubcribes(Guid companyId)
    {
        try
        {
            var data = _context.details_subcribe.Where(d => d.CompanyId.Equals(companyId)).ToList();
            return data;
        }
        catch
        {
            throw;
        }

    }
    public DetailSubcribe UpdateAppsUsed(string id)
    {
        try
        {
            var data = _context.details_subcribe.FirstOrDefault(d => d.Id.ToString().Equals(id));
            if (data != null)
            {
                var remaining = data.AppsTotal-data.AppsUsed;
                data.RemainingApps=remaining;
                data.AppsUsed+=1;
                data.UpdatedBy = "system";
                data.UpdatedAt = DateTime.UtcNow;
                _context.SaveChangesAsync();
                return data;
            }
            else
            {
                throw new CredentialException("Company Id Not Found");
            }
        }
        catch
        {
            throw;
        }
    }
    public DetailSubcribe UpdateDeviceUsed(string id)
    {
        try
        {
            var data = _context.details_subcribe.FirstOrDefault(d => d.Id.ToString().Equals(id));
            if (data != null)
            {
                var remaining = data.DeviceTotal-data.DeviceUsed;
                data.RemainingDevice=remaining;
                data.DeviceUsed+=1;
                data.UpdatedBy = "system";
                data.UpdatedAt = DateTime.UtcNow;
                _context.SaveChangesAsync();
                return data;
            }
            else
            {
                throw new IdentityException("Company Id Not Found");
            }
        }
        catch
        {
            throw;
        }
    }
}