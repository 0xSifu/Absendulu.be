using AbsenDulu.BE.DTO.Company;
using AbsenDulu.BE.Interfaces.IServices;
using AbsenDulu.BE.Models.Company;
using AbsenDulu.BE.Database.Helper.Context;

namespace AbsenDulu.BE.Services;
public class WorkshopService:IWorkshopService
{
    private readonly DataContext _context;
    public WorkshopService(DataContext dataContext)
    {
        _context = dataContext;
    }

    public List<MasterWorkshop> GetWorkshop(Guid request)
    {
        try
        {
            var data = _context.master_workshop.Where(d => d.CompanyId.Equals(request)).ToList();
            return data;
        }
        catch
        {
            throw;
        }

    }

    public MasterWorkshop AddWorkshop(MasterWorkshopDTO request)
    {
        try
        {
            var data = _context.master_workshop.Where(d => d.CompanyId.Equals(request.CompanyId)&& d.WorkshopCode.Equals(request.WorkshopCode)||d.CompanyId.Equals(request.CompanyId)&&d.WorkshopName.Equals(request.WorkshopName)).ToList();
            if(data.Count > 0)
            {
                throw new Exception("data has exists");
            }
            MasterWorkshop workshop = new MasterWorkshop
            {
                Id = Guid.NewGuid(),
                WorkshopCode = request.WorkshopCode,
                WorkshopName = request.WorkshopName,
                CompanyId = request.CompanyId,
                Description = request.Description,
                CreatedBy = request.CreatedBy,
                CreatedAt = DateTime.UtcNow
            };
            _context.master_workshop.Add(workshop);
            _context.SaveChanges();
            return workshop;
        }
        catch
        {
            throw;
        }
    }
    public bool RemoveWorkshop(string Id)
    {
        try
        {
            var data = _context.master_workshop.FirstOrDefault(d => d.Id.ToString().Equals(Id));
            if (data == null)
            {
                throw new Exception("Workshop Id Not Found");
            }
            _context.Remove(data);
            _context.SaveChanges();
            return true;
        }
        catch
        {
            throw;
        }
    }

    public MasterWorkshop UpdateWorkshop(MasterWorkshopDTO request)
    {
        try
        {
            var data = _context.master_workshop.FirstOrDefault(d => d.Id.ToString().Equals(request.Id));
            if (data != null)
            {
                data.WorkshopCode = request.WorkshopCode;
                data.WorkshopName = request.WorkshopName;
                data.Description = request.Description;
                data.UpdatedBy = request.UpdatedBy;
                data.UpdatedDate = DateTime.UtcNow;
                _context.SaveChangesAsync();

                return data;
            }
            else
            {
                throw new Exception("Workshop Id Not Found");
            }
        }
        catch
        {
            throw;
        }
    }

    public List<MasterWorkshop> FindByContains(string request,Guid companyId)
    {
        try
        {
            var data = _context.master_workshop
            .Where(e => e.WorkshopCode.ToLower().Contains(request.ToLower()) || e.WorkshopName.ToLower().Contains(request.ToLower())
            || e.Description.ToLower().Contains(request.ToLower())
            && e.CompanyId== companyId).ToList();
            return data;
        }
        catch (Exception)
        {
            throw;
        }
    }
}