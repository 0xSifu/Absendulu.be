using AbsenDulu.BE.TimeZone;

public class PaginationFilter
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string search { get; set; }
    public DateTime date { get; set; }
    public PaginationFilter()
    {
        this.PageNumber = 1;
        this.PageSize = 10;
        this.search="";
        this.date= TimeZoneUtility.ConvertToWIBTime(DateTime.UtcNow);
    }
    public PaginationFilter(int pageNumber, int pageSize, string search)
    {
        this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
        this.PageSize = pageSize > 10 ? 10 : pageSize;
        this.search = search;
    }
}