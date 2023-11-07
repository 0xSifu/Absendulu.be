using AbsenDulu.BE.Database.Helper.Context;
using AbsenDulu.BE.DTO.Company;
using AbsenDulu.BE.Interfaces.IServices.CompanyData;
using AbsenDulu.BE.Models.Company;

namespace AbsenDulu.BE.Services.CompanyData;
public class ReimbursementService : IReimbursementService
{
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContext;
    public ReimbursementService(DataContext dataContext,IHttpContextAccessor httpContext)
    {
        _context = dataContext;
        _httpContext = httpContext;
    }

    public List<MasterReimbursements> GetReimbursement(Guid companyId)
    {
        try
        {
            var data = _context.master_reimbursements.Where(d => d.CompanyId.Equals(companyId)).ToList();
            return data;
        }
        catch
        {
            throw;
        }

    }

    public MasterReimbursements AddReimbursement(MasterReimbursementDTO request)
    {
        try
        {
            var data = _context.master_reimbursements.Where(d => d.CompanyId.Equals(request.CompanyId)&& d.ReimbursementCode.Equals(request.ReimbursementCode)||d.CompanyId.Equals(request.CompanyId)&&d.ReimbursementName.Equals(request.ReimbursementName)).ToList();
            if(data.Count > 0)
            {
                throw new Exception("data has exists");
            }
            MasterReimbursements Reimbursement = new MasterReimbursements
            {
                Id = Guid.NewGuid(),
                ReimbursementCode = request.ReimbursementCode,
                ReimbursementName = request.ReimbursementName,
                TotalAmount = request.TotalAmount,
                CompanyId = request.CompanyId,
                Company=request.Company,
                CreatedBy = request.CreatedBy,
                CreatedAt = DateTime.UtcNow
            };
            _context.master_reimbursements.Add(Reimbursement);
            _context.SaveChanges();
            return Reimbursement;
        }
        catch
        {
           throw;
        }
    }
    public bool RemoveReimbursement(string Id)
    {
        try
        {
            var data = _context.master_reimbursements.FirstOrDefault(d => d.Id.ToString().Equals(Id));
            if (data == null)
            {
                throw new Exception("Reimbursement Id Not Found");
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

    public MasterReimbursements UpdateReimbursement(MasterReimbursementDTO request)
    {
        try
        {
            var data = _context.master_reimbursements.FirstOrDefault(d => d.Id.Equals(request.Id));
            if (data != null)
            {
                data.ReimbursementCode = request.ReimbursementCode;
                data.ReimbursementName = request.ReimbursementName;
                data.TotalAmount = request.TotalAmount;
                data.UpdatedBy = request.CreatedBy;
                data.UpdatedDate = DateTime.UtcNow;
                _context.SaveChangesAsync();

                return data;
            }
            else
            {
                throw new Exception("Reimbursement Id Not Found");
            }
        }
        catch
        {
            throw;
        }
    }

    public List<MasterReimbursements> FindByContains(string request,Guid companyId)
    {
        try
        {
            var data = _context.master_reimbursements
            .Where(e => e.ReimbursementCode.ToLower().Contains(request.ToLower()) || e.ReimbursementName.ToLower().Contains(request.ToLower())
            && e.CompanyId == companyId).ToList();
            return data;
        }
        catch
        {
            throw;
        }
    }
}