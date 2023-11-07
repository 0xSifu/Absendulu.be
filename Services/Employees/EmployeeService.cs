
using AbsenDulu.BE.Interfaces.IServices;
using AbsenDulu.BE.Database.Helper.Context;
using AbsenDulu.BE.Models.Employee;
using AbsenDulu.BE.DTO;
using AbsenDulu.BE.Interfaces.IServices.PushNotification;

namespace AbsenDulu.BE.Services;
public class EmployeeService:IEmployeeService
{
    private readonly DataContext _context;
    public EmployeeService(DataContext dataContext)
    {
        _context = dataContext;
    }

    public List<Employee> GetEmployeeUsername(string EmployeId,Guid companId)
    {
        try
        {
            var data = _context.employees.Where(d => d.CompanyCode.Equals(companId) && d.EmployeeId.Equals(EmployeId)).ToList();
            return data;
        }
        catch
        {
            throw;
        }

    }

    public List<Employee> GetEmployeeId(string employeeId, Guid companId)
    {
        try
        {
            var data = _context.employees.Where(d => d.CompanyCode.Equals(companId) && d.EmployeeId.Equals(employeeId)).ToList();
            return data;
        }
        catch
        {
            throw;
        }

    }

    public List<Employee> GetEmployee(Guid request)
    {
        try
        {
            var data = _context.employees.Where(d => d.CompanyCode.Equals(request)).ToList();
            return data;
        }
        catch
        {
            throw;
        }

    }
    public Employee GetEmployeeDepartmentCode(Guid request,int employeeId)
    {
        try
        {
            var data = _context.employees.Where(d => d.CompanyCode.Equals(request) && d.Id== employeeId).ToList();
            return data.FirstOrDefault();
        }
        catch
        {
            throw;
        }

    }

    public List<Employee> GetEmployeeById(Guid request,int Id)
    {
        try
        {
            var data = _context.employees.Where(d => d.CompanyCode.Equals(request) && d.Id==Id).ToList();
            return data;
        }
        catch
        {
            throw;
        }

    }
    public Employee AddEmployee(EmployeeDTO request)
    {
        try
        {
            Employee Employee = new Employee
            {
                EmployeeId = request.EmployeeId,
                EmployeeName = request.EmployeeName,
                EmailAddress = request.EmailAddress,
                JoinDate = request.JoinDate,
                EmployeeTypeCode= request.EmployeeTypeCode,
                EmployeeTypeName = request.EmployeeTypeName,
                DepartmentCode = request.DepartmentCode,
                DepartmentName = request.DepartmentName,
                PositionCode = request.PositionCode,
                PositionName = request.PositionName,
                WorkTypeId=request.WorkTypeId,
                WorkTypeName=request.WorkTypeName,
                EffectiveStart=request.EffectiveStart,
                AreaCode = request.AreaCode,
                AreaName = request.AreaName,
                AccountTypeid = request.AccountTypeid,
                Gender = request.Gender,
                Birthday = request.Birthday,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                PostalCode = request.PostalCode,
                National = request.National,
                Religion = request.Religion,
                CompanyCode = request.CompanyCode,
                CompanyName = request.CompanyName,
                CreatedBy = request.CreatedBy,
                CreatedAt = DateTime.UtcNow
            };
            _context.employees.Add(Employee);
            _context.SaveChanges();
            return Employee;
        }
        catch
        {
            throw;
        }
    }
    public bool RemoveEmployee(string Id,string updateBy)
    {
        try
        {
            var data = _context.employees.FirstOrDefault(d => d.Id.ToString().Equals(Id));
            if (data == null)
            {
                throw new ValidationException("Employee Id Not Found");
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
    public Employee UpdateEmployee(EmployeeDTO request)
    {
        try
        {
            var data = _context.employees.FirstOrDefault(d => d.Id.Equals(request.Id));
            if (data != null)
            {
                data.EmployeeId= request.EmployeeId;
                data.EmployeeName = request.EmployeeName;
                data.EmailAddress = request.EmailAddress;
                data.JoinDate = request.JoinDate;
                data.EmployeeTypeCode = request.EmployeeTypeCode;
                data.EmployeeTypeName = request.EmployeeTypeName;
                data.DepartmentCode = request.DepartmentCode;
                data.DepartmentName = request.DepartmentName;
                data.PositionCode = request.PositionCode;
                data.PositionName = request.PositionName;
                data.AreaCode = request.AreaCode;
                data.AreaName = request.AreaName;
                data.AccountTypeid = request.AccountTypeid;
                data.Gender = request.Gender;
                data.Birthday = request.Birthday;
                data.PhoneNumber = request.PhoneNumber;
                data.Address = request.Address;
                data.PostalCode = request.PostalCode;
                data.National = request.National;
                data.Religion = request.Religion;
                data.IsResign = request.IsResign;
                data.ResignDate = request.ResignDate;
                data.EffectiveStart = request.EffectiveStart;
                data.EffectiveEnd = request.EffectiveEnd;
                data.UpdatedBy = request.UpdatedBy;
                data.UpdatedAt = DateTime.UtcNow;
                _context.SaveChangesAsync();
                return data;
            }
            else
            {
                throw new ValidationException("Employee Id Not Found");
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    public Employee ChangeProfilePicture(int id, string url,string user)
    {
        try
        {
            var data = _context.employees.FirstOrDefault(d => d.Id.Equals(id));
            if (data != null)
            {
                data.ProfilePicture=url;
                data.UpdatedBy = user;
                data.UpdatedAt = DateTime.UtcNow;
                _context.SaveChangesAsync();
                return data;
            }
            else
            {
                throw new ValidationException("Employee Id Not Found");
            }
        }
        catch
        {
            throw;
        }
    }


    public List<Employee> FindByContains(string request,Guid companyCode)
    {
        try
        {
            var data = _context.employees
            .Where(e => e.EmployeeId.ToLower().Contains(request.ToLower()) 
            || e.EmployeeName.ToLower().Contains(request.ToLower()) || e.EmployeeTypeName.ToLower().Contains(request.ToLower())
            || e.EmailAddress.ToLower().Contains(request.ToLower()) || e.Gender.ToLower().Contains(request.ToLower())
            || e.PositionCode.ToLower().Contains(request.ToLower()) || e.PositionName.ToLower().Contains(request.ToLower())
            || e.Address.ToLower().Contains(request.ToLower()) || e.AccountTypeid.ToString().ToLower().Contains(request.ToLower())
            || e.AreaCode.ToLower().Contains(request.ToLower()) || e.AreaName.ToLower().Contains(request.ToLower())
            || e.PhoneNumber.ToLower().Contains(request.ToLower()) || e.PostalCode.ToLower().Contains(request.ToLower())
            || e.Religion.ToLower().Contains(request.ToLower())
            || e.EmployeeTypeCode.ToLower().Contains(request.ToLower()) || e.National.ToLower().Contains(request.ToLower())
            || e.WorkTypeName.ToLower().Contains(request.ToLower()) || e.WorkTypeId.ToLower().Contains(request.ToLower())
            || e.DepartmentCode.ToLower().Contains(request.ToLower()) || e.DepartmentName.ToLower().Contains(request.ToLower())
            && e.CompanyCode== companyCode).ToList();
            return data;
        }
        catch (Exception)
        {
            throw;
        }
    }
}