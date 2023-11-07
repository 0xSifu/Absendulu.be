using AbsenDulu.BE.Database.Helper.Context;
using AbsenDulu.BE.DTO.Company;
using AbsenDulu.BE.Interfaces.IServices.CompanyData;
using AbsenDulu.BE.Models.Company;

namespace AbsenDulu.BE.Services.CompanyData;
public class ShiftService : IShiftService
{
    private readonly DataContext _context;

    public ShiftService(DataContext dataContext)
    {
        _context = dataContext;
    }

    public List<MasterShift> GetShift(Guid request)
    {
        try
        {
            var data = _context.master_shift.Where(d => d.CompanyId.Equals(request)).ToList();
            return data;
        }
        catch
        {
            throw;
        }
    }

    public List<MasterShift> GetShiftById(Guid? request)
    {
        try
        {
            var data = _context.master_shift.Where(d => d.Id.Equals(request)).ToList();
            return data;
        }
        catch
        {
            throw;
        }
    }

    public MasterShift AddShift(MasterShiftDTO request)
    {
        try
        {
            var data = _context.master_shift.Where(d => d.CompanyId.Equals(request.CompanyId)&& d.ShiftCode.Equals(request.ShiftCode)||d.CompanyId.Equals(request.CompanyId)&&d.ShiftName.Equals(request.ShiftName)).ToList();
            if(data.Count > 0)
            {
                throw new Exception("data has exists");
            }
            MasterShift shift = new MasterShift
            {
                Id = Guid.NewGuid(),
                ShiftCode=request.ShiftCode,
                ShiftName=request.ShiftName,
                MaximumLate=request.MaximumLate,
                StartWorkTime = request.StartWorkTime,
                EndWorkTime = request.EndWorkTime,
                StartBreakTime = request.StartBreakTime,
                EndBreakTime = request.EndBreakTime,
                WorkDays= request.WorkDays,
                CompanyId = request.CompanyId,
                Company = request.Company,
                CreatedBy = request.CreatedBy,
                CreatedAt = DateTime.UtcNow
            };
            _context.master_shift.Add(shift);
            _context.SaveChanges();
            return shift;
        }
        catch
        {
            throw;
        }
    }

    public bool RemoveShift(Guid Id)
    {
        try
        {
            var data = _context.master_shift.FirstOrDefault(d => d.Id.Equals(Id));
            if (data == null)
            {
                throw new Exception("Shift Id Not Found");
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

    public MasterShift UpdateShift(MasterShiftDTO request)
    {
        try
        {
            var data = _context.master_shift.FirstOrDefault(d => d.Id.Equals(request.Id));
            if (data != null)
            {
                data.ShiftCode = request.ShiftCode;
                data.ShiftName = request.ShiftName;
                data.MaximumLate = request.MaximumLate;
                data.StartWorkTime = request.StartWorkTime;
                data.EndWorkTime = request.EndWorkTime;
                data.StartBreakTime = request.EndBreakTime;
                data.EndBreakTime = request.EndBreakTime;
                data.WorkDays= request.WorkDays;
                data.UpdatedBy = request.UpdatedBy;
                data.UpdatedDate = DateTime.UtcNow;
                _context.SaveChangesAsync();
                return data;
            }
            else
            {
                throw new Exception("Shift Id Not Found");
            }
        }
        catch
        {
            throw;
        }
    }

    public List<MasterShift> FindByContains(string request ,Guid companyId)
    {
        try
        {
            // var searchTime = TimeSpan.Parse(request);
            var data = _context.master_shift.Where(e => e.ShiftCode.ToLower().Contains(request.ToLower())
            || e.ShiftName.ToLower().Contains(request.ToLower())|| e.StartWorkTime.Contains(request)
            || e.EndWorkTime.Contains(request)|| e.StartBreakTime.Contains(request)
            || e.EndBreakTime.Contains(request)
            && e.CompanyId == companyId).ToList();
            return data;
        }
        catch (Exception)
        {
            throw;
        }
    }
}