using AbsenDulu.BE.Database.Helper.Context;
using AbsenDulu.BE.Interfaces.IServices.CompanyData;
using AbsenDulu.BE.Models.Company;

namespace AbsenDulu.BE.Services.CompanyData;
public class CompanyService : ICompanyService
{
    private readonly DataContext _context;
    public CompanyService(DataContext dataContext)
    {
        _context=dataContext;
    }
    public List<MasterCompany> GetCompanies(Guid companyId)
    {
        try
        {
            var data = _context.master_companies.Where(d => d.Id.Equals(companyId)).ToList();
            return data;
        }
        catch (Exception)
        {
            throw;
        }
    }
}