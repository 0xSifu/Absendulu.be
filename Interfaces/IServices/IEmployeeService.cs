using AbsenDulu.BE.DTO;
using AbsenDulu.BE.Models.Employee;
namespace AbsenDulu.BE.Interfaces.IServices;
public interface IEmployeeService
{
    List<Employee> GetEmployee(Guid request);
    List<Employee> GetEmployeeId(string employeename, Guid companId);
    Employee AddEmployee(EmployeeDTO request);
    List<Employee> GetEmployeeById(Guid request, int Id);
    bool RemoveEmployee(string Id,string updateBy);
    Employee UpdateEmployee(EmployeeDTO request);
    List<Employee> FindByContains(string request, Guid companyId);
    List<Employee> GetEmployeeUsername(string request, Guid companyId);
    Employee ChangeProfilePicture(int id, string url, string user);
    Employee GetEmployeeDepartmentCode(Guid request, int employeeId);
}
