namespace AbsenDulu.BE.Models.Calender;
public class CalendarRequest
{
    public int Year { get; set; }
    public List<Holiday> Holidays { get; set; }
}