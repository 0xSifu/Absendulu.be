using AbsenDulu.BE.DTO.Company;
using AbsenDulu.BE.Interfaces.IServices;
using AbsenDulu.BE.Models.Company;
using AbsenDulu.BE.Database.Helper.Context;

namespace AbsenDulu.BE.Services;
public class DepartmentService:IDepartmentService
{
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContext;
    public DepartmentService(DataContext dataContext, IHttpContextAccessor httpContext)
    {
        _context = dataContext;
        _httpContext = httpContext;
    }

    public List<MasterDepartment> GetDepartment(Guid request)
    {
        try
        {
            var data = _context.master_department.Where(d => d.CompanyId.Equals(request)).ToList();
            return data;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public MasterDepartment AddDepartment(MasterDepartmentDTO request)
    {
        try
        {
            var data = _context.master_department.Where(d =>
            d.CompanyId.Equals(request.CompanyId) &&
            d.DepartmentCode.Equals(request.DepartmentCode)||d.CompanyId.Equals(request.CompanyId)&&
            d.DepartmentName.Equals(request.DepartmentName)).ToList();
            if(data.Count > 0)
            {
                throw new Exception("data has exists");
            }
            MasterDepartment department = new MasterDepartment
            {
                Id = Guid.NewGuid(),
                DepartmentCode = request.DepartmentCode,
                DepartmentName = request.DepartmentName,
                CompanyId = request.CompanyId,
                Company = request.Company,
                CreatedBy = request.CreatedBy,
                CreatedAt = DateTime.UtcNow,
                UpdatedDate=null
            };
            _context.master_department.Add(department);
            _context.SaveChanges();
            return department;
        }
        catch
        {
            throw;
        }
    }
    public bool RemoveDepartment(string Id)
    {
        try
        {
            var data = _context.master_department.FirstOrDefault(d => d.Id.ToString().Equals(Id));
            if (data == null)
            {
                throw new Exception("Department Id Not Found");
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

    public MasterDepartment UpdateDepartment(MasterDepartmentDTO request)
    {
        try
        {
            var data = _context.master_department.FirstOrDefault(d => d.Id==request.Id);
            if (data != null)
            {
                data.DepartmentCode = request.DepartmentCode;
                data.DepartmentName = request.DepartmentName;
                data.UpdatedBy = request.UpdatedBy;
                data.UpdatedDate = DateTime.UtcNow;
                _context.SaveChangesAsync();

                return data;
            }
            else
            {
                throw new Exception("Department Id Not Found");
            }
        }
        catch
        {
            throw;
        }
    }

    public List<MasterDepartment> FindByContains(string request,Guid companyId)
    {
        try
        {
            var data = _context.master_department
            .Where(e => e.DepartmentCode.ToLower().Contains(request.ToLower()) || e.DepartmentName.ToLower().Contains(request.ToLower())
            && e.CompanyId == companyId).ToList();
            return data;
        }
        catch
        {
            throw;
        }
    }
}