using AbsenDulu.BE.Models.Company;

namespace AbsenDulu.BE.Interfaces.IServices.CompanyData;
public interface ICompanyService
{
    List<MasterCompany> GetCompanies(Guid companyId);
}