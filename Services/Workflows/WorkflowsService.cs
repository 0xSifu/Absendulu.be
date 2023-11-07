using AbsenDulu.BE.Database.Helper.Context;
using AbsenDulu.BE.Interfaces.IServices.Workflows;
using AbsenDulu.BE.Models.Workflow;

namespace AbsenDulu.BE.Services.Workflows;
public class WorkflowsService: IWorkflowsService
{
    private readonly DataContext _context;
    public WorkflowsService(DataContext dataContext)
    {
        _context=dataContext;
    }

    public List<Workflow> GetWorkflows(Guid CompanyId)
    {
        try
        {
            var data = _context.workflows.Where(d => d.CompanyId.Equals(CompanyId)).ToList();
            return data;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public List<Workflow> GetWorkflowsByDepartment(Guid companyId,string  departmentCode)
    {
        try
        {
            var data = _context.workflows.Where(d => d.CompanyId.Equals(companyId) && d.DepartmentCode== departmentCode).ToList();
            return data;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public List<Workflow> GetWorkflowsByPosition(Guid companyId, string positionCode)
    {
        try
        {
            var data = _context.workflows.Where(d => d.CompanyId.Equals(companyId) && d.PositionCode == positionCode).ToList();
            return data;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public List<Workflow> GetWorkflowsByPositionAndApprovalName(Guid companyId, string positionCode,string ApprovalName)
    {
        try
        {
            var data = _context.workflows.Where(d => d.CompanyId.Equals(companyId) && d.PositionCode == positionCode && d.ApprovalName==ApprovalName).ToList();
            return data;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public Workflow AddWorkflow(Workflow request)
    {
        try
        {
            var data = _context.workflows.Where(d =>
            d.CompanyId.Equals(request.CompanyId) &&
            d.PositionCode.Equals(request.PositionCode) && d.ApprovalName.ToLower()==request.ApprovalName.ToLower()).ToList();
            if (data.Count > 0)
            {
                throw new Exception("data has exists");
            }
            Workflow workflowData = new Workflow
            {
                Id = Guid.NewGuid(),
                DepartmentId = request.DepartmentId,
                DepartmentName = request.DepartmentName,
                DepartmentCode = request.DepartmentCode,
                ApprovalName = request.ApprovalName,
                PositionId = request.PositionId,
                PositionCode = request.PositionCode,
                PositionName = request.PositionName,
                CompanyId = request.CompanyId,
                CreatedBy = request.CreatedBy,
                CreatedAt = DateTime.UtcNow,
            };
            if (request.approver != null && request.approver.Any())
            {
                List<JsonContentApprover> listData = new List<JsonContentApprover>();
                foreach (var item in request.approver)
                {
                    var dataJson= new JsonContentApprover()
                    {
                        PositionCode = item.PositionCode,
                        PositionName =item.PositionName,
                        DepartmentCode=item.DepartmentCode,
                        DepartmentName=item.DepartmentName
                    };
                    listData.Add(dataJson);
                }
                workflowData.approver= listData;
            }
            _context.workflows.Add(workflowData);
            _context.SaveChanges();
            return workflowData;
        }
        catch
        {
            throw;
        }
    }

    public bool RemoveWorkflow(Guid Id)
    {
        try
        {
            var data = _context.workflows.FirstOrDefault(d => d.Id.Equals(Id));
            if (data == null)
            {
                throw new Exception("Workflow Id Not Found");
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

    public Workflow UpdateWorkflow(Workflow request)
    {
        try
        {
            var data = _context.workflows.FirstOrDefault(d => d.Id == request.Id);
            if (data != null)
            {
                data.DepartmentId = request.DepartmentId;
                data.DepartmentCode = request.DepartmentCode;
                data.DepartmentName = request.DepartmentName;
                data.PositionId = request.PositionId;
                data.PositionCode = request.PositionCode;
                data.PositionName = request.PositionName;
                data.UpdatedBy = request.UpdatedBy;
                data.UpdatedAt = DateTime.UtcNow;
            }
            else
            {
                throw new Exception("Department Id Not Found");
            }

            if (request.approver != null && request.approver.Any())
            {
                List<JsonContentApprover> listData = new List<JsonContentApprover>();
                foreach (var item in request.approver)
                {
                    var dataJson = new JsonContentApprover()
                    {
                        PositionCode = item.PositionCode,
                        PositionName = item.PositionName,
                        DepartmentCode = item.DepartmentCode,
                        DepartmentName = item.DepartmentName
                    };
                    listData.Add(dataJson);
                }
                data.approver = listData;
            }

            _context.SaveChangesAsync();
            return data;
        }
        catch
        {
            throw;
        }
    }

    private JsonContentApprover Setproperties(Workflow request)
    {
        var data = new JsonContentApprover()
        {
            
        };
        return data;
    }
}