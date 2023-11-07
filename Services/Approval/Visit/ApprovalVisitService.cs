// using AbsenDulu.BE.Database.Helper.Context;
// using AbsenDulu.BE.DTO.Approval;
// using AbsenDulu.BE.DTO.Notifications;
// using AbsenDulu.BE.Interfaces.IServices;
// using AbsenDulu.BE.Interfaces.IServices.Approval;
// using AbsenDulu.BE.Interfaces.IServices.CompanyData;
// using AbsenDulu.BE.Interfaces.IServices.Notification;
// using AbsenDulu.BE.Interfaces.IServices.PushNotification;
// using AbsenDulu.BE.Interfaces.IServices.Workflows;
// using AbsenDulu.BE.Models.Approval;
// using AbsenDulu.BE.Models.Approval.Visit;
// using AbsenDulu.BE.Models.Workflow;
// using AbsenDulu.BE.Token.Helper;
// using Alachisoft.NCache.Parser;

// namespace AbsenDulu.BE.Services.Approval;
// public class ApprovalVisitService : IApprovalVisitService
// {
//     private readonly DataContext _context;
//     private readonly IEmployeeService _employee;
//     private readonly TokenValidate _tokenConfig;
//     private readonly IWorkflowsService _workflowService;
//     private readonly IRabbitMQService _pushNotification;
//     private readonly INotificationService _notifService;
//     private readonly IApprovalLogService _approvalLogService;
//     public ApprovalVisitService(IApprovalLogService approvalLogService, INotificationService notificationService, IRabbitMQService rabbitMQService, IWorkflowsService workflowsService, DataContext dataContext, IEmployeeService employeeService, TokenValidate tokenValidate)
//     {
//         _context = dataContext;
//         _employee = employeeService;
//         _tokenConfig = tokenValidate;
//         _workflowService = workflowsService;
//         _pushNotification=rabbitMQService;
//         _notifService=notificationService;
//         _approvalLogService =approvalLogService;
//     }

//     public List<ApprovalVisit> GetApprovals()
//     {
//         try
//         {
//             var employee = _employee.GetEmployeeById(_tokenConfig.CompanyId, Convert.ToInt32(_tokenConfig.EmployeeId));
//             var data = _context.approval_visit
//                     .Where(d => d.CompanyId == _tokenConfig.CompanyId && d.Username==employee.FirstOrDefault().EmployeeId)
//                     .ToList();
//             return data;
//         }
//         catch
//         {
//             throw;
//         }
//     }

//     public ApprovalVisit AddApproval(RequestApprovalVisitDTO request)
//     {
//         using (var transaction = _context.Database.BeginTransaction())
//         {
//             try
//             {
//                 var employee = _employee.GetEmployeeById(_tokenConfig.CompanyId, Convert.ToInt32(_tokenConfig.EmployeeId));
//                 var requestor = _employee.GetEmployee(_tokenConfig.CompanyId, Convert.ToInt32(_tokenConfig.EmployeeId));
//                 var approver = _workflowService.GetWorkflowsByPosition(_tokenConfig.CompanyId,employee.FirstOrDefault().PositionCode);
//                 ApprovalVisit Approval = new ApprovalVisit
//                 {
//                     Id = Guid.NewGuid(),
//                     UserAccountId = new Guid(_tokenConfig.Id),
//                     Username = _tokenConfig.UserName,
//                     ReimbursesName = request.ReimburseName,
//                     ReimbursesBudget = request.ReimburseBudget,
//                     ReimbursesAmount = request.ReimburseAmount,
//                     DocumentAttachments = request.Document,
//                     Note = request.Note,
//                     Status = "Waiting Approval",
//                     previous_approver = null,
//                     current_approver = CurrentApproval(approver.FirstOrDefault().approver),
//                     next_approver = NextApproval(approver.FirstOrDefault().approver),
//                     properties = Setproperties(approver.FirstOrDefault().approver),
//                     CompanyId = _tokenConfig.CompanyId,
//                     CreatedAt = DateTime.UtcNow
//                 };
//                 _context.approval_visit.Add(Approval);
//                 _context.SaveChanges();

//                 ApprovalLeaveLogs logs = new ApprovalLeaveLogs
//                 {
//                     Id = Guid.NewGuid(),
//                     ApprovalId = Approval.Id,
//                     RequestorId = employee.FirstOrDefault().Id,
//                     ApproverId = null,
//                     Note = request.Note,
//                     Status = "Waiting Approval",
//                     CompanyId = _tokenConfig.CompanyId,
//                     CreatedAt = DateTime.UtcNow
//                 };
//                 _approvalLogService.AddApprovalLog(logs);

//                 NotificationsDTO notification = new NotificationsDTO
//                 {
//                     Header = "Approvals",
//                     Title = request.ReimburseName,
//                     Message = $"Your Request {request.ReimburseName} with Id : {Approval.Id} Succes Created Status Waiting Approval",
//                     UserId = new Guid(_tokenConfig.Id),
//                     CompanyId = _tokenConfig.CompanyId,
//                 };

//                 _notifService.AddNotification(notification);
//                 _pushNotification.SendMessageToQueue($"employee-{Convert.ToInt32(_tokenConfig.EmployeeId)}", $"Your Request {request.ReimburseName} with Id : {Approval.Id} Succes Created Status Waiting Approval");
//                 transaction.Commit();
//                 return Approval;
//             }
//             catch
//             {
//                 transaction.Rollback();
//                 throw;
//             }
//         }
//     }

//     public ApprovalVisit UpdateApproval(Guid id,ApprovalDTO request)
//     {
//         using (var transaction = _context.Database.BeginTransaction())
//         {
//             try
//             {
//                 var employee = _employee.GetEmployeeById(_tokenConfig.CompanyId, Convert.ToInt32(_tokenConfig.EmployeeId));
//                 var data = _context.approval_visit.FirstOrDefault(d => d.Id.Equals(id));
//                 if (data == null)
//                 {
//                     throw new BadHttpRequestException("No Data Approval");
//                 }
//                 ApprovalVisit Approval = new ApprovalVisit
//                 {
//                     IsApproved=false
//                 };

//                 ApprovalLeaveLogs logs = new ApprovalLeaveLogs
//                 {
//                     Id = Guid.NewGuid(),
//                     ApprovalId = id,
//                     RequestorId = GetEmployeId(data.Username),
//                     ApproverId = Convert.ToInt32(_tokenConfig.EmployeeId),
//                     Note = request.Note,
//                     Status = request.Status,
//                     CompanyId = _tokenConfig.CompanyId,
//                     CreatedAt = DateTime.UtcNow
//                 };
//                 _approvalLogService.AddApprovalLog(logs);

//                 NotificationsDTO notification = new NotificationsDTO
//                 {
//                     Header = "Approvals",
//                     Title = $"Visit To {data.Address}",
//                     Message = $"Reject  Note : {request.Note}",
//                     UserId = new Guid(_tokenConfig.Id),
//                     CompanyId = _tokenConfig.CompanyId,
//                 };

//                 _notifService.AddNotification(notification);
//                 _pushNotification.SendMessageToQueue(_tokenConfig.EmployeeId, $"{request.Status} , Note : {request.Note}");
//                 _context.SaveChangesAsync();
//                 transaction.Commit();
//                 return data;
//             }
//             catch
//             {
//                 transaction.Rollback();
//                 throw;
//             }
//         }
//     }

//     public List<ApprovalVisit> BulkApprove(string note)
//     {
//         using (var transaction = _context.Database.BeginTransaction())
//         {
//             try
//             {
//                 var employee = _employee.GetEmployeeById(_tokenConfig.CompanyId, Convert.ToInt32(_tokenConfig.EmployeeId));
//                 var jsonToCheck = new JsonContentApprover
//                 {
//                     PositionCode=employee.FirstOrDefault().PositionCode,
//                     PositionName = employee.FirstOrDefault().PositionName,
//                     DepartmentCode = employee.FirstOrDefault().DepartmentCode,
//                     DepartmentName = employee.FirstOrDefault().DepartmentName
//                 };
//                 var data = _context.approval_visit
//                         .Where(d => d.current_approver.PositionCode == jsonToCheck.PositionCode
//                         && d.current_approver.PositionName == jsonToCheck.PositionName
//                         && d.current_approver.DepartmentCode == jsonToCheck.DepartmentCode
//                         && d.current_approver.DepartmentName == jsonToCheck.DepartmentName)
//                         .ToList();
//                 if (data.Count < 1 || data == null)
//                 {
//                     throw new BadHttpRequestException("No Data Approval");
//                 }

//                 foreach (var item in data)
//                 {
//                     if (item.properties == null || item.current_approver == null)
//                     {
//                         continue;
//                     }

//                     if (item.current_approver.ToString() != _tokenConfig.EmployeeId)
//                     {
//                         throw new UnauthorizedAccessException("Access Denied");
//                     }

//                     if (item.properties.Count > 0)
//                     {
//                         item.previous_approver = item.current_approver;
//                         item.current_approver = CurrentApproval(item.properties);
//                         item.next_approver = NextApproval(item.properties);
//                         item.Status = $"Waiting Approval";
//                         item.Note = note;
//                         item.properties = Setproperties(item.properties);
//                         item.UpdatedAt = DateTime.UtcNow;
//                     }
//                     else
//                     {
//                         item.previous_approver = item.current_approver;
//                         item.current_approver = null;
//                         item.next_approver = null;
//                         item.Status = $"Approved";
//                         item.Note = note;
//                         item.properties = null;
//                         item.UpdatedAt = DateTime.UtcNow;
//                     }

//                     ApprovalLeaveLogs logs = new ApprovalLeaveLogs
//                     {
//                         Id = Guid.NewGuid(),
//                         ApprovalId = item.Id,
//                         RequestorId = GetEmployeId(item.Username),
//                         ApproverId = Convert.ToInt32(_tokenConfig.EmployeeId),
//                         Note = note,
//                         Status = "Approved",
//                         CompanyId = _tokenConfig.CompanyId,
//                         CreatedAt = DateTime.UtcNow
//                     };
//                     _approvalLogService.AddApprovalLog(logs);


//                     NotificationsDTO notification = new NotificationsDTO
//                     {
//                         Header = "Approvals",
//                         Title = item.ReimbursesName,
//                         Message = $"Approved , Note: {note}",
//                         UserId = new Guid(_tokenConfig.Id),
//                         CompanyId = _tokenConfig.CompanyId,
//                     };

//                     _notifService.AddNotification(notification);
//                     _pushNotification.SendMessageToQueue(_tokenConfig.EmployeeId, $"Approved , Note: {note}");
//                 }

//                 _context.SaveChanges();
//                 transaction.Commit();
//                 return data;
//             }
//             catch
//             {
//                 transaction.Rollback();
//                 throw;
//             }
//         }
//     }

//     public List<ApprovalVisit> BulkReject(string note)
//     {
//         using (var transaction = _context.Database.BeginTransaction())
//         {
//             try
//             {
//                 var employee = _employee.GetEmployeeById(_tokenConfig.CompanyId, Convert.ToInt32(_tokenConfig.EmployeeId));
//                 var jsonToCheck = new JsonContentApprover
//                 {
//                     PositionCode = employee.FirstOrDefault().PositionCode,
//                     PositionName = employee.FirstOrDefault().PositionName,
//                     DepartmentCode = employee.FirstOrDefault().DepartmentCode,
//                     DepartmentName = employee.FirstOrDefault().DepartmentName
//                 };
//                 var data = _context.approval_visit
//                         .Where(d => d.current_approver.PositionCode == jsonToCheck.PositionCode
//                         && d.current_approver.PositionName == jsonToCheck.PositionName
//                         && d.current_approver.DepartmentCode == jsonToCheck.DepartmentCode
//                         && d.current_approver.DepartmentName == jsonToCheck.DepartmentName)
//                         .ToList();
//                 if (data.Count == 0)
//                 {
//                     throw new BadHttpRequestException("No Data Approval");
//                 }

//                 foreach (var item in data)
//                 {
//                     if (item.properties == null || item.current_approver == null)
//                     {
//                         continue;
//                     }

//                     if (item.current_approver.DepartmentCode != _tokenConfig.DepartmentCode && item.current_approver.PositionCode != employee.FirstOrDefault().PositionCode)
//                     {
//                         throw new UnauthorizedAccessException("Access Denied");
//                     }

//                     item.previous_approver = item.current_approver;
//                     item.current_approver = null;
//                     item.next_approver = null;
//                     item.UpdatedAt = DateTime.UtcNow;
//                     item.Status = "Rejected";
//                     item.Note = note;

//                     ApprovalLeaveLogs logs = new ApprovalLeaveLogs
//                     {
//                         Id = Guid.NewGuid(),
//                         ApprovalId = item.Id,
//                         RequestorId = GetEmployeId(item.Username),
//                         ApproverId = Convert.ToInt32(_tokenConfig.EmployeeId),
//                         Note = note,
//                         Status = "Rejected",
//                         CompanyId = _tokenConfig.CompanyId,
//                         CreatedAt = DateTime.UtcNow
//                     };
//                     _approvalLogService.AddApprovalLog(logs);

//                     NotificationsDTO notification = new NotificationsDTO
//                     {
//                         Header = "Approvals",
//                         Title = item.ReimbursesName,
//                         Message = $"Rejected, Note: {note}",
//                         UserId = new Guid(_tokenConfig.Id),
//                         CompanyId = _tokenConfig.CompanyId,
//                     };

//                     _notifService.AddNotification(notification);
//                     _pushNotification.SendMessageToQueue(_tokenConfig.EmployeeId, $"Rejected , Note: {note}");
//                 }

//                 _context.SaveChanges();
//                 transaction.Commit();
//                 return data;
//             }
//             catch
//             {
//                 transaction.Rollback(); // Batalkan transaksi jika terjadi kesalahan
//                 throw;
//             }
//         }
//     }

//     public List<ApprovalVisit> BulkApproveSelected(BulkApprovedDTO request)
//     {
//         using (var transaction = _context.Database.BeginTransaction())
//         {
//             try
//             {
//                 List<ApprovalVisit> result = new List<ApprovalVisit>();
//                 foreach (var item in request.Id)
//                 {
//                     var data = _context.approval_visit.FirstOrDefault(d => d.Id.Equals(item));
//                     if (data.properties == null || data.current_approver == null)
//                     {
//                         continue;
//                     }

//                     if (data.current_approver.ToString() != _tokenConfig.EmployeeId)
//                     {
//                         throw new UnauthorizedAccessException("Access Denied");
//                     }

//                     data.previous_approver = data.current_approver;
//                     data.current_approver = null;
//                     data.next_approver = null;
//                     data.UpdatedAt = DateTime.UtcNow;
//                     data.Status = "Approved";
//                     data.Note = request.Note;

//                     ApprovalLeaveLogs logs = new ApprovalLeaveLogs
//                     {
//                         Id = Guid.NewGuid(),
//                         ApprovalId = data.Id,
//                         RequestorId = GetEmployeId(data.Username),
//                         ApproverId = Convert.ToInt32(_tokenConfig.EmployeeId),
//                         Note = request.Note,
//                         Status = "Approved",
//                         CompanyId = _tokenConfig.CompanyId,
//                         CreatedAt = DateTime.UtcNow
//                     };
//                     _approvalLogService.AddApprovalLog(logs);

//                     NotificationsDTO notification = new NotificationsDTO
//                     {
//                         Header = "Approvals",
//                         Title = data.ReimbursesName,
//                         Message = $"Approved, Note: {request.Note}",
//                         UserId = new Guid(_tokenConfig.Id),
//                         CompanyId = _tokenConfig.CompanyId,
//                     };
//                     result.Add(data);
//                     _notifService.AddNotification(notification);
//                     _pushNotification.SendMessageToQueue(_tokenConfig.EmployeeId, $"Approved , Note: {request.Note}");
//                 }

//                 _context.SaveChanges();
//                 transaction.Commit();
//                 return result;
//             }
//             catch (Exception ex)
//             {
//                 transaction.Rollback(); // Batalkan transaksi jika terjadi kesalahan
//                 throw new Exception("An error occurred while processing approvals.", ex);
//             }
//         }
//     }

//     public List<ApprovalVisit> GetCurrentApprover()
//     {
//         try
//         {
//             var employee = _employee.GetEmployeeById(_tokenConfig.CompanyId, Convert.ToInt32(_tokenConfig.EmployeeId));
//             var jsonToCheck = new JsonContentApprover
//             {
//                 PositionCode = employee.FirstOrDefault().PositionCode,
//                 PositionName = employee.FirstOrDefault().PositionName,
//                 DepartmentCode = employee.FirstOrDefault().DepartmentCode,
//                 DepartmentName = employee.FirstOrDefault().DepartmentName
//             };
//             var data = _context.approval_visit
//                        .Where(d => d.current_approver.PositionCode == jsonToCheck.PositionCode
//                        && d.current_approver.PositionName == jsonToCheck.PositionName
//                        && d.current_approver.DepartmentCode == jsonToCheck.DepartmentCode
//                        && d.current_approver.DepartmentName == jsonToCheck.DepartmentName)
//                        .ToList();
//             return data;
//         }
//         catch
//         {
//             throw;
//         }
//     }

//     public List<ApprovalVisit> BulkRejectSelected(BulkRejectedDTO request)
//     {
//         using (var transaction = _context.Database.BeginTransaction())
//         {
//             try
//             {
//                 List<ApprovalVisit> result = new List<ApprovalVisit>();
//                 foreach (var item in request.Id)
//                 {
//                     var data = _context.approval_visit.FirstOrDefault(d => d.Id.Equals(item));
//                     if (data.properties == null || data.current_approver == null)
//                     {
//                         continue; // Lanjutkan ke item berikutnya jika ada data yang tidak sesuai
//                     }

//                     if (data.current_approver.ToString() != _tokenConfig.EmployeeId)
//                     {
//                         throw new UnauthorizedAccessException("Access Denied");
//                     }

//                     data.previous_approver = data.current_approver;
//                     data.current_approver = null;
//                     data.next_approver = null;
//                     data.UpdatedAt = DateTime.UtcNow;
//                     data.Status = "Rejected";
//                     data.Note = request.Note;

//                     ApprovalLeaveLogs logs = new ApprovalLeaveLogs
//                     {
//                         Id = Guid.NewGuid(),
//                         ApprovalId = data.Id,
//                         RequestorId = GetEmployeId(data.Username),
//                         ApproverId = Convert.ToInt32(_tokenConfig.EmployeeId),
//                         Note = request.Note,
//                         Status = "Rejected",
//                         CompanyId = _tokenConfig.CompanyId,
//                         CreatedAt = DateTime.UtcNow
//                     };
//                     _approvalLogService.AddApprovalLog(logs);

//                     NotificationsDTO notification = new NotificationsDTO
//                     {
//                         Header = "Approvals",
//                         Title = data.ReimbursesName,
//                         Message = $"Rejected, Note: {request.Note}",
//                         UserId = new Guid(_tokenConfig.Id),
//                         CompanyId = _tokenConfig.CompanyId,
//                     };
//                     result.Add(data);
//                     _notifService.AddNotification(notification);
//                     _pushNotification.SendMessageToQueue(_tokenConfig.EmployeeId, $"Rejected , Note: {request.Note}");
//                 }

//                 _context.SaveChanges();
//                 transaction.Commit();
//                 return result;
//             }
//             catch (Exception ex)
//             {
//                 transaction.Rollback(); // Batalkan transaksi jika terjadi kesalahan
//                 throw new Exception("An error occurred while processing approvals.", ex);
//             }
//         }
//     }

//     private JsonContentApprover CurrentApproval(List<JsonContentApprover> approval)
//     {
//         if (approval.Count > 0)
//         {
//             return approval.FirstOrDefault();
//         }
//         return null;
//     }
//     private JsonContentApprover NextApproval(List<JsonContentApprover> approval)
//     {
//         if (approval.Count > 0)
//         {
//             if (approval.Count < 2)
//             {
//                 return null;
//             }
//             return approval[1];
//         }
//         return null;
//     }

//     private List<JsonContentApprover> Setproperties(List<JsonContentApprover> request)
//     {
//         if (request.Count > 0)
//         {
//             return request.Where(val => val != CurrentApproval(request)).ToList();
//         }
//         return null;
//     }

//     private int GetEmployeId(string username)
//     {
//         var employee = _employee.GetEmployeeId(username, _tokenConfig.CompanyId);
//         return employee.FirstOrDefault().Id;
//     }
// }