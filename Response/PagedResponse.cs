using AbsenDulu.BE.Response;

public class PagedResponse<T> : Responses<T>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public Uri FirstPage { get; set; }
    public Uri LastPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }
    public Uri NextPage { get; set; }
    public Uri PreviousPage { get; set; }
    public PagedResponse(T data, int pageNumber, int pageSize,int totalPages,int totalRecords,string message , bool success, string[]? error)
    {
        this.PageNumber = pageNumber;
        this.PageSize = pageSize;
        this.TotalPages = totalPages;
        this.TotalRecords = totalRecords;
        this.Data = data;
        this.Message = message;
        this.Succeeded = success;
        this.Errors = error;
    }
}