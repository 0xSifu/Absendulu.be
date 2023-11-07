using AbsenDulu.BE.DTO;
using AbsenDulu.BE.Filters;
using AbsenDulu.BE.Interfaces.IServices;
using AbsenDulu.BE.Models.Employee;
using AbsenDulu.BE.Response;
using AbsenDulu.BE.Token.Helper;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AbsenDulu.BE.Controllers;

[ApiController]
public class EmployeeController : ControllerBase
{
    private IEmployeeService _service;
    private TokenValidate _token;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public EmployeeController(IWebHostEnvironment webHostEnvironment,TokenValidate tokenValidate, IEmployeeService service)
    {
        _service = service;
        _token = tokenValidate;
        _webHostEnvironment=webHostEnvironment;
    }

    [HttpPost]
    [Authorize]
    [Route("[controller]/export")]
    [ApiExplorerSettings(GroupName = "v1")]
    public IActionResult Excel(List<EmployeeDTO> request)
    {
        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("Employee");
            var currentRow = 1;
            worksheet.Cell(currentRow, 1).Value = "Employee Id";
            worksheet.Cell(currentRow, 2).Value = "Employee Name";
            worksheet.Cell(currentRow, 3).Value = "Department Code";
            worksheet.Cell(currentRow, 4).Value = "Department Name";
            worksheet.Cell(currentRow, 5).Value = "Position Code";
            worksheet.Cell(currentRow, 6).Value = "Position Name";
            worksheet.Cell(currentRow, 7).Value = "Employee Type Code";
            worksheet.Cell(currentRow, 8).Value = "Employee Type Name";
            worksheet.Cell(currentRow, 9).Value = "Company Code";
            worksheet.Cell(currentRow, 10).Value = "Company Name";
            worksheet.Cell(currentRow, 11).Value = "PIC";
            worksheet.Cell(currentRow, 12).Value = "Gender";
            worksheet.Cell(currentRow, 13).Value = "Birthday";
            worksheet.Cell(currentRow, 14).Value = "PhoneNumber";
            worksheet.Cell(currentRow, 15).Value = "Address";
            worksheet.Cell(currentRow, 16).Value = "Postal Code";
            worksheet.Cell(currentRow, 17).Value = "National";
            worksheet.Cell(currentRow, 18).Value = "Religion";
            worksheet.Cell(currentRow, 19).Value = "Email Address";
            worksheet.Cell(currentRow, 20).Value = "Bank Name";
            worksheet.Cell(currentRow, 21).Value = "Bank Account";
            worksheet.Cell(currentRow, 22).Value = "Payment Method";
            worksheet.Cell(currentRow, 23).Value = "BPJS Employee No";
            worksheet.Cell(currentRow, 24).Value = "BPJS Employee Start Pay";
            worksheet.Cell(currentRow, 25).Value = "BPJS Employee End Pay";
            worksheet.Cell(currentRow, 26).Value = "BPJS HealthCare No";
            worksheet.Cell(currentRow, 27).Value = "BPJS HealthCare Start Pay";
            worksheet.Cell(currentRow, 28).Value = "BPJS HealthCare End Pay";
            worksheet.Cell(currentRow, 29).Value = "NPWP No";
            worksheet.Cell(currentRow, 30).Value = "Tax Start Pay";
            worksheet.Cell(currentRow, 31).Value = "Tax End Pay";
            worksheet.Cell(currentRow, 32).Value = "Profile Photo";
            worksheet.Cell(currentRow, 33).Value = "Work Type Id";
            worksheet.Cell(currentRow, 34).Value = "Work Type Name";
            worksheet.Cell(currentRow, 35).Value = "Area Code";
            worksheet.Cell(currentRow, 36).Value = "Area Name";
            worksheet.Cell(currentRow, 37).Value = "Is Resign";
            worksheet.Cell(currentRow, 38).Value = "Join Date";
            worksheet.Cell(currentRow, 39).Value = "Resign Date";
            worksheet.Cell(currentRow, 40).Value = "Effective Start";
            worksheet.Cell(currentRow, 41).Value = "Effective End";
            worksheet.Cell(currentRow, 42).Value = "Account Type Id";

            foreach (var employee in request)
            {
                currentRow++;
                worksheet.Cell(currentRow, 1).Value = employee.EmployeeId;
                worksheet.Cell(currentRow, 2).Value = employee.EmployeeName;
                worksheet.Cell(currentRow, 3).Value = employee.DepartmentCode;
                worksheet.Cell(currentRow, 4).Value = employee.DepartmentName;
                worksheet.Cell(currentRow, 5).Value = employee.PositionCode;
                worksheet.Cell(currentRow, 6).Value = employee.PositionName;
                worksheet.Cell(currentRow, 7).Value = employee.EmployeeTypeCode;
                worksheet.Cell(currentRow, 8).Value = employee.EmployeeTypeName;
                worksheet.Cell(currentRow, 9).Value = employee.CompanyCode.ToString();
                worksheet.Cell(currentRow, 10).Value = employee.CompanyName;
                worksheet.Cell(currentRow, 11).Value = employee.PIC;
                worksheet.Cell(currentRow, 12).Value = employee.Gender;
                worksheet.Cell(currentRow, 13).Value = employee.Birthday;
                worksheet.Cell(currentRow, 14).Value = employee.PhoneNumber;
                worksheet.Cell(currentRow, 15).Value = employee.Address;
                worksheet.Cell(currentRow, 16).Value = employee.PostalCode;
                worksheet.Cell(currentRow, 17).Value = employee.National;
                worksheet.Cell(currentRow, 18).Value = employee.Religion;
                worksheet.Cell(currentRow, 19).Value = employee.EmailAddress;
                worksheet.Cell(currentRow, 20).Value = employee.BankName;
                worksheet.Cell(currentRow, 21).Value = employee.BankAccount;
                worksheet.Cell(currentRow, 22).Value = employee.PaymentMethod;
                worksheet.Cell(currentRow, 23).Value = employee.BpjsEmployeeNo;
                worksheet.Cell(currentRow, 24).Value = employee.BpjsEmployeeStartPay;
                worksheet.Cell(currentRow, 25).Value = employee.BpjsEmployeeEndPay;
                worksheet.Cell(currentRow, 26).Value = employee.BpjsHealthCareNo;
                worksheet.Cell(currentRow, 27).Value = employee.BpjsHealthCareStartPay;
                worksheet.Cell(currentRow, 28).Value = employee.BpjsHealthCareEndPay;
                worksheet.Cell(currentRow, 29).Value = employee.NPWPNo;
                worksheet.Cell(currentRow, 30).Value = employee.TaxStartPay;
                worksheet.Cell(currentRow, 31).Value = employee.TaxEndPay;
                worksheet.Cell(currentRow, 32).Value = employee.ProfilePicture;
                worksheet.Cell(currentRow, 33).Value = employee.WorkTypeId;
                worksheet.Cell(currentRow, 34).Value = employee.WorkTypeName;
                worksheet.Cell(currentRow, 35).Value = employee.AreaCode;
                worksheet.Cell(currentRow, 36).Value = employee.AreaName;
                worksheet.Cell(currentRow, 37).Value = employee.IsResign;
                worksheet.Cell(currentRow, 38).Value = employee.JoinDate;
                worksheet.Cell(currentRow, 39).Value = employee.ResignDate;
                worksheet.Cell(currentRow, 40).Value = employee.EffectiveStart;
                worksheet.Cell(currentRow, 41).Value = employee.EffectiveEnd;
                worksheet.Cell(currentRow, 42).Value = employee.AccountTypeid;
            }

            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                var content = stream.ToArray();

                return File(
                    content,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "Employee_Report.xlsx");
            }
        }
    }

    [HttpGet]
    [Authorize]
    [Route("[controller]/GetPagination")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public async Task<ActionResult<List<Employee>>> GetArea([FromQuery] PaginationFilter filter)
    {
        try
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.search.ToLower());
            var responseContext = string.IsNullOrEmpty(filter.search) ? await Task.Run(() => _service.GetEmployee(_token.CompanyId)) : await Task.Run(() => _service.FindByContains(filter.search, _token.CompanyId));
            var totalRecords = responseContext.Count;
            var totalPages = (int)Math.Ceiling((double)totalRecords / filter.PageSize);
            var paging = responseContext
            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize).ToList();
            return Ok(new PagedResponse<List<Employee>>(paging, validFilter.PageNumber, validFilter.PageSize, totalPages, totalRecords, "suceess", true, null));
        }
        catch
        {
            throw;
        }
    }

    [HttpGet]
    [Authorize]
    [Route("[controller]/GetEmployee")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public async Task<ActionResult<List<Employee>>> GetEmployee()
    {
        try
        {
            var responseContext = await Task.Run(() => _service.GetEmployee(_token.CompanyId));
            return Ok(new ResponseMessage<List<Employee>> { IsError = false, Message = "Success", Data = responseContext });
        }
        catch
        {
            throw;
        }
    }


    [HttpPost]
    [Authorize]
    [Route("[controller]/AddEmployee")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public async Task<ActionResult> AddEmployee(EmployeeDTO request)
    {
        try
        {
            request.CompanyCode = _token.CompanyId;
            request.CompanyName = _token.CompanyName;
            request.CreatedBy = _token.UserName;
            var responseContext = await Task.Run(() => _service.AddEmployee(request));
            return Created("", new ResponseMessage<Employee> { IsError = false, Message = "Success", Data = responseContext });
        }
        catch
        {
            throw;
        }
    }

    [HttpPut("[controller]/{id}")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public IActionResult Put(int id, [FromBody] EmployeeDTO request)
    {
        try
        {
            request.Id = id;
            request.UpdatedBy = _token.UserName;
            var responseContext = _service.UpdateEmployee(request); 
            return Ok(new ResponseMessage<Employee> { IsError = false, Message = "Success", Data = responseContext });
        }
        catch
        {
            throw;
        }
    }

    [HttpPut("[controller]/picture/ChangeProfilePicture")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public IActionResult ChangeProfilePicture([FromQuery] string url)
    {
        try
        {
            var responseContext = _service.ChangeProfilePicture(Convert.ToInt32(_token.EmployeeId),url,_token.UserName);
            return Ok(new ResponseMessage<Employee> { IsError = false, Message = "Success", Data = responseContext });
        }
        catch
        {
            throw;
        }
    }

    [HttpGet("[controller]/GetProfileDetail")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public IActionResult GetProfileDetail()
    {
        try
        {
            var responseContext = _service.GetEmployeeById(_token.CompanyId,Convert.ToInt32(_token.Id));
            return Ok(new ResponseMessage<Employee> { IsError = false, Message = "Success", Data = responseContext.FirstOrDefault() });
        }
        catch
        {
            throw;
        }
    }

    [HttpDelete("[controller]/{id}")]
    [ServiceFilter(typeof(AuthorizeTokenFilterService))]
    [ApiExplorerSettings(GroupName = "v1")]
    public IActionResult Delete(string id)
    {
        try
        {
            var responseContext = _service.RemoveEmployee(id, _token.UserName);
            return Ok("Success Delete Id : " + id);
        }
        catch
        {
            throw;
        }
    }
}