using System.Text.RegularExpressions;
using AbsenDulu.BE.Database.Helper.Context;
using AbsenDulu.BE.Interfaces.IServices;
using AbsenDulu.BE.Interfaces.IServices.Approval;
using AbsenDulu.BE.Models.Approval;
using AbsenDulu.BE.Models.Employee;
using AbsenDulu.BE.Token.Helper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace AbsenDulu.BE.Services.Approval;
public class ApprovalRemainingService: IApprovalRemainingService
{
    private readonly IEmployeeService _employee;
    private readonly DataContext _context;
    private readonly TokenValidate _tokenConfig;

    public ApprovalRemainingService(TokenValidate tokenValidate,  IEmployeeService employeeService , DataContext dataContext)
    {
        _employee=employeeService;
        _tokenConfig=tokenValidate;
        _context=dataContext;
    }
    public BalanceLeave GetRemaining(string LeaveName)
    {
        try
        {
            double balanceCount = 0;
            string pattern = $".*{Regex.Escape("Annual")}.*";
            var employee = GetEmployeeById();
            DateTime joinDate = employee.EffectiveStart;
            DateTime currentDate = DateTime.UtcNow;
            TimeSpan? timeSpan = currentDate - joinDate;
            double totalBulan = (currentDate.Year - joinDate.Year) * 12 + (currentDate.Month - joinDate.Month);

            var balance = _context.master_leave.Where(d => d.CompanyId == _tokenConfig.CompanyId && d.LeaveName == LeaveName);
            if(balance==null)
            {
                throw new Exception("data Leave Not Found");
            }
            var totalDaysSum = _context.approval_leaves
                .Where(d => d.CompanyId == _tokenConfig.CompanyId && d.LeaveName == LeaveName && d.Username == employee.EmployeeId && d.Status == "Approved")
                .Sum(d => d.TotalDays);
            balanceCount = totalBulan - totalDaysSum;

            var data = new BalanceLeave
            {
                EmployeName = employee.EmployeeName,
                ApprovalName = LeaveName,
                Balance = Regex.IsMatch(LeaveName, pattern, RegexOptions.IgnoreCase) ? totalBulan : balance.FirstOrDefault().TotalDays == null ? 0 : balance.FirstOrDefault().TotalDays,
                Remaining = Regex.IsMatch(LeaveName, pattern, RegexOptions.IgnoreCase) ? balanceCount : balance.FirstOrDefault().TotalDays == null ?0 :balance.FirstOrDefault().TotalDays - totalDaysSum
            };

            return data;
        }
        catch
        {
            throw;
        }
    }

    private Employee GetEmployeeById()
    {
        return _employee.GetEmployeeById(_tokenConfig.CompanyId, Convert.ToInt32(_tokenConfig.EmployeeId)).FirstOrDefault();
    }

    private Employee? GetEmployeeId()
    {
        return _employee.GetEmployeeId(GetEmployeeById().EmployeeId, _tokenConfig.CompanyId).FirstOrDefault();
    }
}