using AbsenDulu.BE.DTO.Company;
using AbsenDulu.BE.Interfaces.IServices;
using AbsenDulu.BE.Models.Company;
using AbsenDulu.BE.Database.Helper.Context;

namespace AbsenDulu.BE.Services;
public class LeaveService:ILeaveService
{
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContext;
    public LeaveService(DataContext dataContext, IHttpContextAccessor httpContext)
    {
        _context = dataContext;
        _httpContext = httpContext;
    }

    public List<MasterLeave> GetLeave(Guid request)
    {
        try
        {
            var data = _context.master_leave.Where(d => d.CompanyId.Equals(request)).ToList();
            return data;
        }
        catch
        {
            throw;
        }

    }

    public MasterLeave AddLeave(MasterLeaveDTO request)
    {
        try
        {
            var data = _context.master_leave.Where(d => d.CompanyId.Equals(request.CompanyId)&& d.LeaveCode.Equals(request.LeaveCode)||d.CompanyId.Equals(request.CompanyId)&&d.LeaveName.Equals(request.LeaveName)).ToList();
            if(data.Count > 0)
            {
                throw new Exception("data has exists");
            }
            MasterLeave Leave = new MasterLeave
            {
                Id = Guid.NewGuid(),
                LeaveCode = request.LeaveCode,
                LeaveName = request.LeaveName,
                TotalDays = request.TotalDays,
                LeaveMoreThanBalance = request.LeaveMoreThanBalance,
                CompanyId = request.CompanyId,
                Company = request.Company,
                CreatedBy = request.CreatedBy,
                CreatedAt = DateTime.UtcNow
            };
            _context.master_leave.Add(Leave);
            _context.SaveChanges();
            return Leave;
        }
        catch
        {
            throw;
        }
    }
    public bool RemoveLeave(string Id)
    {
        try
        {
            var data = _context.master_leave.FirstOrDefault(d => d.Id.ToString().Equals(Id));
            if (data == null)
            {
                throw new Exception("Leave Id Not Found");
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

    public MasterLeave UpdateLeave(MasterLeaveDTO request)
    {
        try
        {
            var data = _context.master_leave.FirstOrDefault(d => d.Id.ToString().Equals(request.Id));
            if (data != null)
            {
                data.LeaveCode = request.LeaveCode;
                data.LeaveName = request.LeaveName;
                data.TotalDays = request.TotalDays;
                data.LeaveMoreThanBalance = request.LeaveMoreThanBalance;
                data.UpdatedBy = request.UpdatedBy;
                data.UpdatedDate = DateTime.UtcNow;
                _context.SaveChangesAsync();

                return data;
            }
            else
            {
                throw new Exception("Leave Id Not Found");
            }
        }
        catch
        {
            throw;
        }
    }

    public List<MasterLeave> FindByContains(string request,Guid companyId)
    {
        try
        {
            var data = _context.master_leave
            .Where(e => e.LeaveCode.ToLower().Contains(request.ToLower()) || e.LeaveName.ToLower().Contains(request.ToLower())
            || e.TotalDays.ToString().Contains(request)
            && e.CompanyId == companyId).ToList();
           return data;
        }
        catch (Exception)
        {
            throw;
        }
    }
}