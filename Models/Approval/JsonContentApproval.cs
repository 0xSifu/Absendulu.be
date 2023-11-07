using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AbsenDulu.BE.Models.Approval;
[Keyless]
public class JsonContentApproval
{
    [JsonProperty("approver")]
    public int[] Approver { get; set; }

    [JsonProperty("CurrentApprovalName")]
    public string CurrentApprovalName { get; set; }

    [JsonProperty("NextApprovalName")]
    public string NextApprovalName { get; set; }

    [JsonProperty("Document")]
    public string Document { get; set; }

    [JsonProperty("FromDate")]
    public DateTime FromDate { get; set; }

    [JsonProperty("ToDate")]
    public DateTime ToDate { get; set; }

    [JsonProperty("Total")]
    public int Total { get; set; }

    [JsonProperty("Note")]
    public string Note { get; set; }

    [JsonProperty("Status")]
    public string Status { get; set; }

}
