using AbsenDulu.BE.Database.Helper.Context;
using AbsenDulu.BE.DTO.Approval;
using AbsenDulu.BE.DTO.Notifications;
using AbsenDulu.BE.Interfaces.IServices;
using AbsenDulu.BE.Interfaces.IServices.Approval;
using AbsenDulu.BE.Interfaces.IServices.Notification;
using AbsenDulu.BE.Interfaces.IServices.PushNotification;
using AbsenDulu.BE.Interfaces.IServices.Workflows;
using AbsenDulu.BE.Models.Approval;
using AbsenDulu.BE.Models.Workflow;
using AbsenDulu.BE.Token.Helper;

namespace AbsenDulu.BE.Services.Approval;
public class ApprovalWorkShopService: IApprovalWorkShopService
{
    private readonly DataContext _context;
    private readonly IEmployeeService _employee;
    private readonly TokenValidate _tokenConfig;
    private readonly IWorkshopService _masterWorkShopService;
    private readonly BalanceLeave _balance;
    private readonly IWorkflowsService _workflowService;
    private readonly IRabbitMQService _pushNotification;
    private readonly INotificationService _notifService;
    private readonly IApprovalWorkShopLogService _approvalLogService;
    public ApprovalWorkShopService(IApprovalWorkShopLogService approvalLogService, INotificationService notificationService, IRabbitMQService rabbitMQService, IWorkflowsService workflowsService, BalanceLeave balanceLeave, IWorkshopService workshopService, DataContext dataContext, IEmployeeService employeeService, TokenValidate tokenValidate)
    {
        _context = dataContext;
        _employee = employeeService;
        _tokenConfig = tokenValidate;
        _masterWorkShopService=workshopService;
        _balance = balanceLeave;
        _workflowService = workflowsService;
        _pushNotification=rabbitMQService;
        _notifService=notificationService;
        _approvalLogService =approvalLogService;
    }

    public List<ApprovalWorkShop> GetApprovals()
    {
        try
        {
            CheckRole();
            var employee = _employee.GetEmployeeById(_tokenConfig.CompanyId, Convert.ToInt32(_tokenConfig.EmployeeId));
            var jsonToCheck = new JsonContentApprover
            {
                PositionCode = employee.FirstOrDefault().PositionCode,
                PositionName = employee.FirstOrDefault().PositionName,
                DepartmentCode = employee.FirstOrDefault().DepartmentCode,
                DepartmentName = employee.FirstOrDefault().DepartmentName
            };
            var data = _context.approval_workshop
                    .Where(d => d.CompanyId == _tokenConfig.CompanyId && d.Username==employee.FirstOrDefault().EmployeeId)
                    .ToList();
            return data;
        }
        catch
        {
            throw;
        }
    }

    public ApprovalWorkShop AddApproval(RequestApprovalWorkShopDTO request)
    {
        using (var transaction = _context.Database.BeginTransaction())
        {
            try
            {
                CheckRole();
                int TotalDays = request.WorkShopStart.Date.Day - request.WorkShopEnd.Date.Day;
                var employee = _employee.GetEmployeeById(_tokenConfig.CompanyId, Convert.ToInt32(_tokenConfig.EmployeeId));
                var approver = _workflowService.GetWorkflowsByPositionAndApprovalName(_tokenConfig.CompanyId,employee.FirstOrDefault().PositionCode,"WorkShop");
                if (approver == null || approver.Count < 1)
                {
                    throw new Exception("Approver Not Set");
                }
                ApprovalWorkShop Approval = new ApprovalWorkShop
                {
                    Id = Guid.NewGuid(),
                    WorkShopName = request.WorkShopName,
                    WorkShopAddress = request.WorkShopAddress,
                    UserAccountId = new Guid(_tokenConfig.Id),
                    Username = _tokenConfig.UserName,
                    DocumentAttachments = request.Document,
                    Note = request.Note,
                    Status = "Waiting Approval",
                    WorkShopStart = request.WorkShopStart.ToUniversalTime(),
                    WorkShopEnd = request.WorkShopEnd.ToUniversalTime(),
                    previous_approver = null,
                    current_approver = CurrentApproval(approver.FirstOrDefault().approver),
                    next_approver = NextApproval(approver.FirstOrDefault().approver),
                    properties = Setproperties(approver.FirstOrDefault().approver),
                    CompanyId = _tokenConfig.CompanyId,
                    CreatedAt = DateTime.UtcNow
                };
                _context.approval_workshop.Add(Approval);
                _context.SaveChanges();

                ApprovalWorkShopLogs logs = new ApprovalWorkShopLogs
                {
                    Id = Guid.NewGuid(),
                    ApprovalId = Approval.Id,
                    RequestorId = employee.FirstOrDefault().Id,
                    ApproverId = null,
                    Note = request.Note,
                    Status = "Waiting Approval",
                    CompanyId = _tokenConfig.CompanyId,
                    CreatedAt = DateTime.UtcNow
                };
                _approvalLogService.AddApprovalLog(logs);

                NotificationsDTO notification = new NotificationsDTO
                {
                    Header = "Approvals",
                    Title = request.WorkShopName,
                    Message = $"Your Request {request.WorkShopName} with Id : {Approval.Id} Succes Created Status Waiting Approval",
                    UserId = new Guid(_tokenConfig.Id),
                    CompanyId = _tokenConfig.CompanyId,
                };

                _notifService.AddNotification(notification);
                _pushNotification.SendMessageToQueue($"employee-{Convert.ToInt32(_tokenConfig.EmployeeId)}", $"Your Request {request.WorkShopName} with Id : {Approval.Id} Succes Created Status Waiting Approval");
                transaction.Commit();
                return Approval;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }

    public ApprovalWorkShop UpdateApproval(Guid id,ApprovalDTO request)
    {
        using (var transaction = _context.Database.BeginTransaction())
        {
            try
            {
                CheckRole();
                var employee = _employee.GetEmployeeById(_tokenConfig.CompanyId, Convert.ToInt32(_tokenConfig.EmployeeId));
                var jsonToCheck = new JsonContentApprover
                {
                    PositionCode = employee.FirstOrDefault().PositionCode,
                    PositionName = employee.FirstOrDefault().PositionName,
                    DepartmentCode = employee.FirstOrDefault().DepartmentCode,
                    DepartmentName = employee.FirstOrDefault().DepartmentName
                };
                var data2 = _context.approval_workshop
                        .Where(d => d.current_approver.PositionCode == jsonToCheck.PositionCode
                        && d.current_approver.PositionName == jsonToCheck.PositionName
                        && d.current_approver.DepartmentCode == jsonToCheck.DepartmentCode
                        && d.current_approver.DepartmentName == jsonToCheck.DepartmentName)
                        .ToList();
                var data = _context.approval_workshop.FirstOrDefault(d => d.Id.Equals(id));
                if (data.properties == null || data.current_approver == null)
                {
                    throw new BadHttpRequestException("No Data Approval");
                }
                ApprovalWorkShop Approval = new ApprovalWorkShop
                {
                    Note = request.Note,
                    Status =request.Status,
                    previous_approver = data.current_approver,
                    current_approver = CurrentApproval(data.properties),
                    next_approver = NextApproval(data.properties),
                    properties = Setproperties(data.properties),
                };

                if (data2 == null || data2.Count < 1)
                {
                    throw new UnauthorizedAccessException("Access Denied");
                }
                if (data != null)
                {
                    if (request.Status == "Rejected")
                    {
                        data.previous_approver =data.current_approver;
                        data.current_approver = null;
                        data.next_approver = null;
                        data.UpdatedAt = DateTime.UtcNow;
                        data.Status = request.Status;
                        data.Note = request.Note;
                    }
                    else
                    {
                        if (data.properties.Count > 0)
                        {
                            data.previous_approver = data.current_approver;
                            data.current_approver = CurrentApproval(data.properties);
                            data.next_approver = NextApproval(data.properties);
                            data.Status = $"Waiting Approval";
                            data.Note = request.Note;
                            data.properties = Setproperties(data.properties);
                            data.UpdatedAt = DateTime.UtcNow;
                        }
                        else
                        {
                            data.previous_approver = data.current_approver;
                            data.current_approver = null;
                            data.next_approver = null;
                            data.Status = $"Approved";
                            data.Note = request.Note;
                            data.properties = null;
                            data.UpdatedAt = DateTime.UtcNow;
                        }
                    }
                }
                else
                {
                    throw new Exception("Approval Id Not Found");
                }

                ApprovalWorkShopLogs logs = new ApprovalWorkShopLogs
                {
                    Id = Guid.NewGuid(),
                    ApprovalId = id,
                    RequestorId = GetEmployeId(data.Username),
                    ApproverId = Convert.ToInt32(_tokenConfig.EmployeeId),
                    Note = request.Note,
                    Status = request.Status,
                    CompanyId = _tokenConfig.CompanyId,
                    CreatedAt = DateTime.UtcNow
                };
                _approvalLogService.AddApprovalLog(logs);

                NotificationsDTO notification = new NotificationsDTO
                {
                    Header = "Approvals",
                    Title = data.WorkShopName,
                    Message = $"{request.Status} Note : {request.Note}",
                    UserId = new Guid(_tokenConfig.Id),
                    CompanyId = _tokenConfig.CompanyId,
                };

                _notifService.AddNotification(notification);
                _pushNotification.SendMessageToQueue(_tokenConfig.EmployeeId, $"{request.Status} , Note : {request.Note}");
                _context.SaveChangesAsync();
                transaction.Commit();
                return data;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }

    public List<ApprovalWorkShop> BulkApprove(string note)
    {
        using (var transaction = _context.Database.BeginTransaction())
        {
            try
            {
                CheckRole();
                var employee = _employee.GetEmployeeById(_tokenConfig.CompanyId, Convert.ToInt32(_tokenConfig.EmployeeId));
                var jsonToCheck = new JsonContentApprover
                {
                    PositionCode=employee.FirstOrDefault().PositionCode,
                    PositionName = employee.FirstOrDefault().PositionName,
                    DepartmentCode = employee.FirstOrDefault().DepartmentCode,
                    DepartmentName = employee.FirstOrDefault().DepartmentName
                };
                var data = _context.approval_workshop
                        .Where(d => d.current_approver.PositionCode == jsonToCheck.PositionCode
                        && d.current_approver.PositionName == jsonToCheck.PositionName
                        && d.current_approver.DepartmentCode == jsonToCheck.DepartmentCode
                        && d.current_approver.DepartmentName == jsonToCheck.DepartmentName)
                        .ToList();
                if (data.Count < 1 || data == null)
                {
                    throw new BadHttpRequestException("No Data Approval");
                }

                foreach (var item in data)
                {
                    if (item.properties == null || item.current_approver == null)
                    {
                        continue;
                    }

                    if (item.current_approver.PositionName!= _tokenConfig.Role)
                    {
                        throw new UnauthorizedAccessException("Access Denied");
                    }

                    if (item.properties.Count > 0)
                    {
                        item.previous_approver = item.current_approver;
                        item.current_approver = CurrentApproval(item.properties);
                        item.next_approver = NextApproval(item.properties);
                        item.Status = $"Waiting Approval";
                        item.Note = note;
                        item.properties = Setproperties(item.properties);
                        item.UpdatedAt = DateTime.UtcNow;
                    }
                    else
                    {
                        item.previous_approver = item.current_approver;
                        item.current_approver = null;
                        item.next_approver = null;
                        item.Status = $"Approved";
                        item.Note = note;
                        item.properties = null;
                        item.UpdatedAt = DateTime.UtcNow;
                    }

                    ApprovalWorkShopLogs logs = new ApprovalWorkShopLogs
                    {
                        Id = Guid.NewGuid(),
                        ApprovalId = item.Id,
                        RequestorId = GetEmployeId(item.Username),
                        ApproverId = Convert.ToInt32(_tokenConfig.EmployeeId),
                        Note = note,
                        Status = "Approved",
                        CompanyId = _tokenConfig.CompanyId,
                        CreatedAt = DateTime.UtcNow
                    };
                    _approvalLogService.AddApprovalLog(logs);


                    NotificationsDTO notification = new NotificationsDTO
                    {
                        Header = "Approvals",
                        Title = item.WorkShopName,
                        Message = $"Approved , Note: {note}",
                        UserId = new Guid(_tokenConfig.Id),
                        CompanyId = _tokenConfig.CompanyId,
                    };

                    _notifService.AddNotification(notification);
                    _pushNotification.SendMessageToQueue(_tokenConfig.EmployeeId, $"Approved , Note: {note}");
                }

                _context.SaveChanges();
                transaction.Commit();
                return data;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }

    public List<ApprovalWorkShop> BulkReject(string note)
    {
        using (var transaction = _context.Database.BeginTransaction())
        {
            try
            {
                CheckRole();
                var employee = _employee.GetEmployeeById(_tokenConfig.CompanyId, Convert.ToInt32(_tokenConfig.EmployeeId));
                var jsonToCheck = new JsonContentApprover
                {
                    PositionCode = employee.FirstOrDefault().PositionCode,
                    PositionName = employee.FirstOrDefault().PositionName,
                    DepartmentCode = employee.FirstOrDefault().DepartmentCode,
                    DepartmentName = employee.FirstOrDefault().DepartmentName
                };
                var data = _context.approval_workshop
                        .Where(d => d.current_approver.PositionCode == jsonToCheck.PositionCode
                        && d.current_approver.PositionName == jsonToCheck.PositionName
                        && d.current_approver.DepartmentCode == jsonToCheck.DepartmentCode
                        && d.current_approver.DepartmentName == jsonToCheck.DepartmentName)
                        .ToList();
                if (data.Count == 0)
                {
                    throw new BadHttpRequestException("No Data Approval");
                }

                foreach (var item in data)
                {
                    if (item.properties == null || item.current_approver == null)
                    {
                        continue;
                    }

                    if (item.current_approver.PositionName != _tokenConfig.Role)
                    {
                        throw new UnauthorizedAccessException("Access Denied");
                    }

                    item.previous_approver = item.current_approver;
                    item.current_approver = null;
                    item.next_approver = null;
                    item.UpdatedAt = DateTime.UtcNow;
                    item.Status = "Rejected";
                    item.Note = note;

                    ApprovalWorkShopLogs logs = new ApprovalWorkShopLogs
                    {
                        Id = Guid.NewGuid(),
                        ApprovalId = item.Id,
                        RequestorId = GetEmployeId(item.Username),
                        ApproverId = Convert.ToInt32(_tokenConfig.EmployeeId),
                        Note = note,
                        Status = "Rejected",
                        CompanyId = _tokenConfig.CompanyId,
                        CreatedAt = DateTime.UtcNow
                    };
                    _approvalLogService.AddApprovalLog(logs);

                    NotificationsDTO notification = new NotificationsDTO
                    {
                        Header = "Approvals",
                        Title = item.WorkShopName,
                        Message = $"Rejected, Note: {note}",
                        UserId = new Guid(_tokenConfig.Id),
                        CompanyId = _tokenConfig.CompanyId,
                    };

                    _notifService.AddNotification(notification);
                    _pushNotification.SendMessageToQueue(_tokenConfig.EmployeeId, $"Rejected , Note: {note}");
                }

                _context.SaveChanges();
                transaction.Commit();
                return data;
            }
            catch
            {
                transaction.Rollback(); // Batalkan transaksi jika terjadi kesalahan
                throw;
            }
        }
    }

    public List<ApprovalWorkShop> BulkApproveSelected(BulkApprovedDTO request)
    {
        using (var transaction = _context.Database.BeginTransaction())
        {
            try
            {
                CheckRole();
                List<ApprovalWorkShop> result = new List<ApprovalWorkShop>();
                foreach (var item in request.Id)
                {
                    var data = _context.approval_workshop.FirstOrDefault(d => d.Id.Equals(item));
                    if (data.properties == null || data.current_approver == null)
                    {
                        continue;
                    }

                    if (data.current_approver.PositionName != _tokenConfig.Role)
                    {
                        throw new UnauthorizedAccessException("Access Denied");
                    }

                    data.previous_approver = data.current_approver;
                    data.current_approver = null;
                    data.next_approver = null;
                    data.UpdatedAt = DateTime.UtcNow;
                    data.Status = "Approved";
                    data.Note = request.Note;

                    ApprovalWorkShopLogs logs = new ApprovalWorkShopLogs
                    {
                        Id = Guid.NewGuid(),
                        ApprovalId = data.Id,
                        RequestorId = GetEmployeId(data.Username),
                        ApproverId = Convert.ToInt32(_tokenConfig.EmployeeId),
                        Note = request.Note,
                        Status = "Approved",
                        CompanyId = _tokenConfig.CompanyId,
                        CreatedAt = DateTime.UtcNow
                    };
                    _approvalLogService.AddApprovalLog(logs);

                    NotificationsDTO notification = new NotificationsDTO
                    {
                        Header = "Approvals",
                        Title = data.WorkShopName,
                        Message = $"Approved, Note: {request.Note}",
                        UserId = new Guid(_tokenConfig.Id),
                        CompanyId = _tokenConfig.CompanyId,
                    };
                    result.Add(data);
                    _notifService.AddNotification(notification);
                    _pushNotification.SendMessageToQueue(_tokenConfig.EmployeeId, $"Approved , Note: {request.Note}");
                }

                _context.SaveChanges();
                transaction.Commit();
                return result;
            }
            catch
            {
                transaction.Rollback(); // Batalkan transaksi jika terjadi kesalahan
                throw;
            }
        }
    }

    public List<ApprovalWorkShop> GetCurrentApprover()
    {
        try
        {
            CheckRole();
            var employee = _employee.GetEmployeeById(_tokenConfig.CompanyId, Convert.ToInt32(_tokenConfig.EmployeeId));
            var jsonToCheck = new JsonContentApprover
            {
                PositionCode = employee.FirstOrDefault().PositionCode,
                PositionName = employee.FirstOrDefault().PositionName,
                DepartmentCode = employee.FirstOrDefault().DepartmentCode,
                DepartmentName = employee.FirstOrDefault().DepartmentName
            };
            var data = _context.approval_workshop
                       .Where(d => d.current_approver.PositionCode == jsonToCheck.PositionCode
                       && d.current_approver.PositionName == jsonToCheck.PositionName
                       && d.current_approver.DepartmentCode == jsonToCheck.DepartmentCode
                       && d.current_approver.DepartmentName == jsonToCheck.DepartmentName)
                       .ToList();
            return data;
        }
        catch
        {
            throw;
        }
    }

    public List<ApprovalWorkShop> BulkRejectSelected(BulkRejectedDTO request)
    {
        using (var transaction = _context.Database.BeginTransaction())
        {
            try
            {
                CheckRole();
                List<ApprovalWorkShop> result = new List<ApprovalWorkShop>();
                foreach (var item in request.Id)
                {
                    var data = _context.approval_workshop.FirstOrDefault(d => d.Id.Equals(item));
                    if (data.properties == null || data.current_approver == null)
                    {
                        continue; // Lanjutkan ke item berikutnya jika ada data yang tidak sesuai
                    }

                    if (data.current_approver.PositionName != _tokenConfig.Role)
                    {
                        throw new UnauthorizedAccessException("Access Denied");
                    }

                    data.previous_approver = data.current_approver;
                    data.current_approver = null;
                    data.next_approver = null;
                    data.UpdatedAt = DateTime.UtcNow;
                    data.Status = "Rejected";
                    data.Note = request.Note;

                    ApprovalWorkShopLogs logs = new ApprovalWorkShopLogs
                    {
                        Id = Guid.NewGuid(),
                        ApprovalId = data.Id,
                        RequestorId = GetEmployeId(data.Username),
                        ApproverId = Convert.ToInt32(_tokenConfig.EmployeeId),
                        Note = request.Note,
                        Status = "Rejected",
                        CompanyId = _tokenConfig.CompanyId,
                        CreatedAt = DateTime.UtcNow
                    };
                    _approvalLogService.AddApprovalLog(logs);

                    NotificationsDTO notification = new NotificationsDTO
                    {
                        Header = "Approvals",
                        Title = data.WorkShopName,
                        Message = $"Rejected, Note: {request.Note}",
                        UserId = new Guid(_tokenConfig.Id),
                        CompanyId = _tokenConfig.CompanyId,
                    };
                    result.Add(data);
                    _notifService.AddNotification(notification);
                    _pushNotification.SendMessageToQueue(_tokenConfig.EmployeeId, $"Rejected , Note: {request.Note}");
                }

                _context.SaveChanges();
                transaction.Commit();
                return result;
            }
            catch (Exception ex)
            {
                transaction.Rollback(); // Batalkan transaksi jika terjadi kesalahan
                throw new Exception("An error occurred while processing approvals.", ex);
            }
        }
    }

    private JsonContentApprover CurrentApproval(List<JsonContentApprover> approval)
    {
        if (approval.Count > 0)
        {
            return approval.FirstOrDefault();
        }
        return null;
    }
    private JsonContentApprover NextApproval(List<JsonContentApprover> approval)
    {
        if (approval.Count > 0)
        {
            if (approval.Count < 2)
            {
                return null;
            }
            return approval[1];
        }
        return null;
    }

    private List<JsonContentApprover> Setproperties(List<JsonContentApprover> request)
    {
        if (request.Count > 0)
        {
            return request.Where(val => val != CurrentApproval(request)).ToList();
        }
        return null;
    }

    private int GetEmployeId(string username)
    {
        var employee = _employee.GetEmployeeId(username, _tokenConfig.CompanyId);
        return employee.FirstOrDefault().Id;
    }

    private void CheckRole()
    {
        if (_tokenConfig.Role.ToLower() == "admin")
        {
            throw new BadHttpRequestException("Admin Not have Approval");
        }
        return;
    }
}