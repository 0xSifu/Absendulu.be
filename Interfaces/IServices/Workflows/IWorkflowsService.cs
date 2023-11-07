using AbsenDulu.BE.Models.Workflow;

namespace AbsenDulu.BE.Interfaces.IServices.Workflows;
public interface IWorkflowsService
{
    List<Workflow> GetWorkflows(Guid companyId);
    List<Workflow> GetWorkflowsByDepartment(Guid companyId, string departmentCode);
    List<Workflow> GetWorkflowsByPosition(Guid companyId, string positionCode);
    List<Workflow> GetWorkflowsByPositionAndApprovalName(Guid companyId, string positionCode, string ApprovalName);
    Workflow AddWorkflow(Workflow request);
    bool RemoveWorkflow(Guid Id);
    Workflow UpdateWorkflow(Workflow request);


}