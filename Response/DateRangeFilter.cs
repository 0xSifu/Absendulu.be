namespace AbsenDulu.BE.Response;
public class DateRangeFilter
{
    public string StartDate { get; set; }
    public string ToDate { get; set; }
    public DateRangeFilter()
    {
        this.StartDate = DateTime.UtcNow.ToString("yyyy-MM-dd");
        this.ToDate = DateTime.UtcNow.ToString("yyyy-MM-dd");
    }
    public DateRangeFilter(int pageNumber, int pageSize)
    {
        this.StartDate = string.IsNullOrEmpty(StartDate) ? DateTime.UtcNow.ToString("yyyy-MM-dd") : StartDate;
        this.ToDate = string.IsNullOrEmpty(ToDate) ? DateTime.UtcNow.ToString("yyyy-MM-dd") : ToDate;
    }
}