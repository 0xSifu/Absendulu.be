using AbsenDulu.BE.DTO.Company;
using AbsenDulu.BE.Interfaces.IServices;
using AbsenDulu.BE.Models.Company;
using AbsenDulu.BE.Database.Helper.Context;

namespace AbsenDulu.BE.Services;
public class PositionService:IPositionService
{
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContext;
    public PositionService(DataContext dataContext, IHttpContextAccessor httpContext)
    {
        _context = dataContext;
        _httpContext = httpContext;
    }

    public List<MasterPosition> GetPosition(Guid request)
    {
        try
        {
            var data = _context.master_position.Where(d => d.CompanyId.Equals(request)).ToList();
            return data;
        }
        catch
        {
            throw;
        }
    }

    public List<MasterPosition> GetPositionByDepartment(Guid request,Guid department)
    {
        try
        {
            var data = _context.master_position.Where(d => d.CompanyId.Equals(request)&& d.DepartmentCode.Equals(department)).ToList();
            return data;
        }
        catch
        {
            throw;
        }
    }

    public MasterPosition AddPosition(MasterPositionDTO request)
    {
        try
        {
            var data = _context.master_position.Where(d => d.CompanyId.Equals(request.CompanyId)&&  d.PositionCode.Equals(request.PositionCode)||d.CompanyId.Equals(request.CompanyId)&&d.PositionName.Equals(request.PositionName)).ToList();
            if(data.Count > 0)
            {
                throw new Exception("data has exists");
            }
            MasterPosition position = new MasterPosition
            {
                Id = Guid.NewGuid(),
                DepartmentCode=request.DepartmentCode,
                DepartmentName=request.DepartmentName,
                DepartmentId = request.DepartmentId,
                PositionCode = request.PositionCode,
                PositionName = request.PositionName,
                CompanyId = request.CompanyId,
                CompanyName = request.Company,
                CreatedBy = request.CreatedBy,
                CreatedAt = DateTime.UtcNow
            };
            _context.master_position.Add(position);
            _context.SaveChanges();
            return position;
        }
        catch
        {
            throw;
        }
    }
    public bool RemovePosition(string Id)
    {
        try
        {
            var data = _context.master_position.FirstOrDefault(d => d.Id.ToString().Equals(Id));
            if (data == null)
            {
                throw new Exception("Position Id Not Found");
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

    public MasterPosition UpdatePosition(MasterPositionDTO request)
    {
        try
        {
            var data = _context.master_position.FirstOrDefault(d => d.Id.Equals(request.Id));
            if (data != null)
            {
                data.DepartmentCode = request.DepartmentCode;
                data.DepartmentName = request.DepartmentName;
                data.PositionCode = request.PositionCode;
                data.PositionName = request.PositionName;
                data.UpdatedBy = request.UpdatedBy;
                data.UpdatedAt = DateTime.UtcNow;
                _context.SaveChangesAsync();

                return data;
            }
            else
            {
                throw new Exception("Position Id Not Found");
            }
        }
        catch
        {
            throw;
        }
    }

    public List<MasterPosition> FindByContains(string request ,Guid companyId)
    {
        try
        {
            var data = _context.master_position.Where(e => e.PositionCode.ToLower().Contains(request) || e.PositionName.ToLower().Contains(request)
            && e.CompanyId== companyId).ToList();
            return data;
        }
        catch (Exception)
        {
            throw;
        }
    }
}