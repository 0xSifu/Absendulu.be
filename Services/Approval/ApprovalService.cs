// using AbsenDulu.BE.Database.Helper.Context;
// using AbsenDulu.BE.DTO.Approval;
// using AbsenDulu.BE.DTO.Notifications;
// using AbsenDulu.BE.Interfaces.IServices;
// using AbsenDulu.BE.Interfaces.IServices.Approval;
// using AbsenDulu.BE.Interfaces.IServices.Notification;
// using AbsenDulu.BE.Interfaces.IServices.PushNotification;
// using AbsenDulu.BE.Models.Approval;
// using AbsenDulu.BE.Token.Helper;

// namespace AbsenDulu.BE.Services.Approval;
// public class ApprovalService : IApprovalService
// {
//     private readonly DataContext _context;
//     private readonly IEmployeeService _employee;
//     private readonly TokenValidate _tokenConfig;
//     private readonly IRabbitMQService _pushNotification;
//     private readonly INotificationService _notifService;
//     private readonly IApprovalLogService _approvalLogService;

//     public ApprovalService(IApprovalLogService approvalLogService, INotificationService notificationService, DataContext dataContext, IEmployeeService employeeService, TokenValidate tokenValidate, IRabbitMQService rabbitMQService)
//     {
//         _context = dataContext;
//         _employee = employeeService;
//         _tokenConfig = tokenValidate;
//         _pushNotification = rabbitMQService;
//         _pushNotification = rabbitMQService;
//         _notifService = notificationService;
//         _approvalLogService = approvalLogService;
//     }

//     public List<Approvals> FindByContains(string request, Guid companyId, string username)
//     {
//         try
//         {
//             // JsonContentApproval properties = JsonConvert.DeserializeObject<JsonContentApproval>(request.properties);
//             // var data = _context.approvals
//             // .Where(e => e.properties.Status.ToLower().Contains(request.ToLower())
//             // && e.CompanyId == companyId && e.Username==username).ToList();
//             var item = new List<Approvals>();
//             return item;
//         }
//         catch
//         {
//             throw;
//         }
//     }
//     public List<Approvals> GetApprovals(Guid companyId, int employeeId)
//     {
//         try
//         {
//             var data = _context.approvals.Where(d => d.CompanyId.Equals(companyId) && d.EmployeeId == employeeId).ToList();
//             return data;
//         }
//         catch
//         {
//             throw;
//         }
//     }

//     public List<Approvals> GetCurrentApprover(Guid companyId, int employeeId)
//     {
//         try
//         {
//             var data = _context.approvals.Where(d => d.CompanyId.Equals(companyId) && d.CurrentApprover == employeeId).ToList();
//             return data;
//         }
//         catch
//         {
//             throw;
//         }
//     }

//     public List<Approvals> BulkRejectSelected(BulkRejectedDTO request)
//     {
//         using (var transaction = _context.Database.BeginTransaction())
//         {
//             try
//             {
//                 List<Approvals> result = new List<Approvals>();
//                 foreach (var item in request.Id)
//                 {
//                     var data = _context.approvals.FirstOrDefault(d => d.Id.Equals(item));
//                     if (data.properties == null || data.CurrentApprover == null)
//                     {
//                         continue; // Lanjutkan ke item berikutnya jika ada data yang tidak sesuai
//                     }

//                     if (data.CurrentApprover.ToString() != _tokenConfig.EmployeeId)
//                     {
//                         throw new UnauthorizedAccessException("Access Denied");
//                     }

//                     RequestApprovalDTO properties = new RequestApprovalDTO
//                     {
//                         ApprovalType = data.ApprovalType,
//                         Approver = data.properties.Approver,
//                         Document = data.properties.Document,
//                         FromDate = data.properties.FromDate,
//                         ToDate = data.properties.ToDate,
//                         Total = data.properties.Total,
//                         Status = data.properties.Status,
//                         Note = data.properties.Note
//                     };

//                     data.CurrentApprover = null;
//                     data.NextApprover = null;
//                     data.UpdatedAt = DateTime.UtcNow;
//                     data.properties.Status = "Rejected";
//                     data.properties.Note = request.Note;

//                     ApprovalsLogs logs = new ApprovalsLogs
//                     {
//                         Id = Guid.NewGuid(),
//                         ApprovalId = data.Id,
//                         RequestorId = data.EmployeeId,
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
//                         Title = data.ApprovalType,
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

//     public List<Approvals> BulkApproveSelected(BulkApprovedDTO request)
//     {
//         using (var transaction = _context.Database.BeginTransaction())
//         {
//             try
//             {
//                 List<Approvals> result = new List<Approvals>();
//                 foreach (var item in request.Id)
//                 {
//                     var data = _context.approvals.FirstOrDefault(d => d.Id.Equals(item));
//                     if (data.properties == null || data.CurrentApprover == null)
//                     {
//                         continue; // Lanjutkan ke item berikutnya jika ada data yang tidak sesuai
//                     }

//                     if (data.CurrentApprover.ToString() != _tokenConfig.EmployeeId)
//                     {
//                         throw new UnauthorizedAccessException("Access Denied");
//                     }

//                     RequestApprovalDTO properties = new RequestApprovalDTO
//                     {
//                         ApprovalType = data.ApprovalType,
//                         Approver = data.properties.Approver,
//                         Document = data.properties.Document,
//                         FromDate = data.properties.FromDate,
//                         ToDate = data.properties.ToDate,
//                         Total = data.properties.Total,
//                         Status = data.properties.Status,
//                         Note = data.properties.Note
//                     };

//                     data.CurrentApprover = null;
//                     data.NextApprover = null;
//                     data.UpdatedAt = DateTime.UtcNow;
//                     data.properties.Status = "Approved";
//                     data.properties.Note = request.Note;

//                     ApprovalsLogs logs = new ApprovalsLogs
//                     {
//                         Id = Guid.NewGuid(),
//                         ApprovalId = data.Id,
//                         RequestorId = data.EmployeeId,
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
//                         Title = data.ApprovalType,
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

//     public List<Approvals> BulkReject(string note)
//     {
//         using (var transaction = _context.Database.BeginTransaction())
//         {
//             try
//             {
//                 var data = _context.approvals.Where(d => d.CurrentApprover.Equals(Convert.ToInt32(_tokenConfig.EmployeeId))).ToList();
//                 if (data.Count == 0)
//                 {
//                     throw new BadHttpRequestException("No Data Approval");
//                 }

//                 foreach (var item in data)
//                 {
//                     if (item.properties == null || item.CurrentApprover == null)
//                     {
//                         continue; // Lanjutkan ke item berikutnya jika ada data yang tidak sesuai
//                     }

//                     if (item.CurrentApprover.ToString() != _tokenConfig.EmployeeId)
//                     {
//                         throw new UnauthorizedAccessException("Access Denied");
//                     }

//                     RequestApprovalDTO properties = new RequestApprovalDTO
//                     {
//                         ApprovalType = item.ApprovalType,
//                         Approver = item.properties.Approver,
//                         Document = item.properties.Document,
//                         FromDate = item.properties.FromDate,
//                         ToDate = item.properties.ToDate,
//                         Total = item.properties.Total,
//                         Status = item.properties.Status,
//                         Note = item.properties.Note
//                     };

//                     item.CurrentApprover = null;
//                     item.NextApprover = null;
//                     item.UpdatedAt = DateTime.UtcNow;
//                     item.properties.Status = "Rejected";
//                     item.properties.Note = note;

//                     ApprovalsLogs logs = new ApprovalsLogs
//                     {
//                         Id = Guid.NewGuid(),
//                         ApprovalId = item.Id,
//                         RequestorId = item.EmployeeId,
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
//                         Title = item.ApprovalType,
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

//     public List<Approvals> BulkApprove(string note)
//     {
//         using (var transaction = _context.Database.BeginTransaction())
//         {
//             try
//             {
//                 var data = _context.approvals.Where(d => d.CurrentApprover.Equals(Convert.ToInt32(_tokenConfig.EmployeeId))).ToList();
//                 if (data.Count < 1 || data==null)
//                 {
//                     throw new BadHttpRequestException("No Data Approval");
//                 }

//                 foreach (var item in data)
//                 {
//                     if (item.properties == null || item.CurrentApprover == null)
//                     {
//                         continue; // Lanjutkan ke item berikutnya jika ada data yang tidak sesuai
//                     }

//                     if (item.CurrentApprover.ToString() != _tokenConfig.EmployeeId)
//                     {
//                         throw new UnauthorizedAccessException("Access Denied");
//                     }

//                     RequestApprovalDTO properties = new RequestApprovalDTO
//                     {
//                         ApprovalType = item.ApprovalType,
//                         Approver = item.properties.Approver,
//                         Document = item.properties.Document,
//                         FromDate = item.properties.FromDate,
//                         ToDate = item.properties.ToDate,
//                         Total = item.properties.Total,
//                         Status = item.properties.Status,
//                         Note = item.properties.Note
//                     };

//                     if (CurrentApproval(item.properties.Approver) > 0)
//                     {
//                         item.CurrentApprover = CurrentApproval(item.properties.Approver) == 0 ? null : CurrentApproval(item.properties.Approver);
//                         item.NextApprover = NextApproval(item.properties.Approver) == 0 ? null : NextApproval(item.properties.Approver);
//                         item.properties.Status = $"Waiting Approval {EmployeeNameById(_tokenConfig.CompanyId, CurrentApproval(item.properties.Approver))}";
//                         item.properties.Note = note;
//                         // Perbaikan: Sesi sebelumnya item.properties diubah menjadi item.properties = Setproperties(properties)
//                         item.properties = Setproperties(properties);
//                         item.UpdatedAt = DateTime.UtcNow;
//                     }
//                     else
//                     {
//                         item.CurrentApprover = null;
//                         item.NextApprover = null;
//                         item.properties.Status = $"Approve By {_tokenConfig.UserName}";
//                         item.properties.Note = note;
//                         // Perbaikan: Sesi sebelumnya item.properties diubah menjadi item.properties = null
//                         item.properties = null;
//                         item.UpdatedAt = DateTime.UtcNow;
//                     }

//                     ApprovalsLogs logs = new ApprovalsLogs
//                     {
//                         Id = Guid.NewGuid(),
//                         ApprovalId = item.Id,
//                         RequestorId = item.EmployeeId,
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
//                         Title = item.ApprovalType,
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
//                 transaction.Rollback(); // Batalkan transaksi jika terjadi kesalahan
//                 throw;
//             }
//         }
//     }


//     public Approvals UpdateApproval(ApprovalDTO request)
//     {
//         using (var transaction = _context.Database.BeginTransaction())
//         {
//             try
//             {
//                 var data = _context.approvals.FirstOrDefault(d => d.Id.Equals(request.Id));

//                 if(data.properties==null || data.CurrentApprover==null)
//                 {
//                     throw new BadHttpRequestException("No Data Approval");
//                 }
//                 RequestApprovalDTO properties = new RequestApprovalDTO 
//                 {
//                     ApprovalType=data.ApprovalType,
//                     Approver=data.properties.Approver,
//                     Document=data.properties.Document,
//                     FromDate=data.properties.FromDate,
//                     ToDate=data.properties.ToDate,
//                     Total=data.properties.Total,
//                     Status=data.properties.Status,
//                     Note=data.properties.Note
//                 };
//                 // if (data.properties.Approver[0].ToString() != _tokenConfig.EmployeeId)

//                 if (data.CurrentApprover.ToString() != _tokenConfig.EmployeeId)
//                 {
//                     throw new UnauthorizedAccessException("Access Denied");
//                 }
//                 if (data != null)
//                 {
//                     if (request.Status == "Rejected")
//                     {
//                         data.CurrentApprover = null;
//                         data.NextApprover = null;
//                         data.UpdatedAt = DateTime.UtcNow;
//                         data.properties.Status = request.Status;
//                         data.properties.Note = request.Note;
//                     }
//                     else
//                     {
//                         if (CurrentApproval(data.properties.Approver) > 0)
//                         {
//                             data.CurrentApprover = CurrentApproval(data.properties.Approver) == 0 ? null : CurrentApproval(data.properties.Approver);
//                             data.NextApprover = NextApproval(data.properties.Approver) == 0 ? null : NextApproval(data.properties.Approver);
//                             data.properties.Status = $"Waiting Approval {EmployeeNameById(_tokenConfig.CompanyId, CurrentApproval(data.properties.Approver))}";
//                             data.properties.Note = request.Note;
//                             data.properties = Setproperties(properties);
//                             data.UpdatedAt = DateTime.UtcNow;
//                         }
//                         else
//                         {
//                             data.CurrentApprover = null;
//                             data.NextApprover = null;
//                             data.properties.Status = $"Approve By {_tokenConfig.UserName}";
//                             data.properties.Note = request.Note;
//                             // data.properties = null;
//                             data.UpdatedAt = DateTime.UtcNow;
//                         }
//                     }
//                 }
//                 else
//                 {
//                     throw new Exception("Approval Id Not Found");
//                 }

//                 ApprovalsLogs logs = new ApprovalsLogs
//                 {
//                     Id = Guid.NewGuid(),
//                     ApprovalId = request.Id,
//                     RequestorId = data.EmployeeId,
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
//                     Title = data.ApprovalType,
//                     Message = $"{request.Status} Note : {request.Note}",
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
//     public Approvals AddApproval(RequestApprovalDTO request)
//     {
//         using (var transaction = _context.Database.BeginTransaction())
//         {
//             try
//             {
//                 Approvals Approval = new Approvals
//                 {
//                     Id = Guid.NewGuid(),
//                     ApprovalType = request.ApprovalType,
//                     UserAccountId = new Guid(_tokenConfig.Id),
//                     Username = _tokenConfig.UserName,
//                     EmployeeId = Convert.ToInt32(_tokenConfig.EmployeeId),
//                     CurrentApprover = CurrentApproval(request.Approver)==0 ? null: CurrentApproval(request.Approver),
//                     NextApprover = NextApproval(request.Approver)==0 ? null : NextApproval(request.Approver),
//                     properties = Setproperties(request),
//                     CompanyId = _tokenConfig.CompanyId,
//                     CreatedAt = DateTime.UtcNow
//                 };
//                 _context.approvals.Add(Approval);
//                 _context.SaveChanges();

//                 ApprovalsLogs logs = new ApprovalsLogs
//                 {
//                     Id = Guid.NewGuid(),
//                     ApprovalId = Approval.Id,
//                     RequestorId = Approval.EmployeeId,
//                     ApproverId = CurrentApproval(request.Approver),
//                     Note = request.Note,
//                     Status = "Waiting Approval ",
//                     CompanyId = _tokenConfig.CompanyId,
//                     CreatedAt = DateTime.UtcNow
//                 };
//                 _approvalLogService.AddApprovalLog(logs);

//                 NotificationsDTO notification = new NotificationsDTO
//                 {
//                     Header = "Approvals",
//                     Title = request.ApprovalType,
//                     Message = $"Your Request {request.ApprovalType} with Id : {Approval.Id} Succes Created Status Waiting Approval",
//                     UserId = new Guid(_tokenConfig.Id),
//                     CompanyId = _tokenConfig.CompanyId,
//                 };

//                 _notifService.AddNotification(notification);
//                 _pushNotification.SendMessageToQueue(_tokenConfig.EmployeeId, $"Your Request {request.ApprovalType} with Id : {Approval.Id} Succes Created Status Waiting Approval");
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

//     private int CurrentApproval(int[] approval)
//     {
//         if (approval.Length > 0)
//         {
//             return approval[0];
//         }
//         return 0;
//     }
//     private int NextApproval(int[] approval)
//     {
//         if (approval.Length > 0)
//         {
//             if (approval.Length < 2)
//             {
//                 return 0;
//             }
//             return approval[1];
//         }
//         return 0;
//     }

//     private JsonContentApproval Setproperties(RequestApprovalDTO request)
//     {
//         var data = new JsonContentApproval()
//         {
//             Approver = request.Approver.Where(val => val != CurrentApproval(request.Approver)).ToArray(),
//             // Approver = request.properties.Approver = request.properties.Approver.Where(val => val != CurrentApproval(request.properties.Approver)).ToArray(),
//             CurrentApprovalName = EmployeeNameById(_tokenConfig.CompanyId, CurrentApproval(request.Approver)),
//             NextApprovalName = EmployeeNameById(_tokenConfig.CompanyId, NextApproval(request.Approver)),
//             Document = request.Document,
//             FromDate = request.FromDate,
//             ToDate = request.ToDate,
//             Total = request.Total,
//             Note = request.Note,
//             Status = request.Status
//         };
//         return data;
//     }

//     private string EmployeeNameById(Guid CompanyId, int Id)
//     {
//         try
//         {
//             var data = _employee.GetEmployeeById(CompanyId, Id).ToList();
//             if(data.Count < 1)
//             {
//                 return "";
//             }
//             return data.FirstOrDefault().EmployeeName;
//         }
//         catch
//         {
//             throw;
//         }
//     }
// }