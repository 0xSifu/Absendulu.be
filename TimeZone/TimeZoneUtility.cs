namespace AbsenDulu.BE.TimeZone;
public class TimeZoneUtility
{
    private static TimeZoneInfo WIB = TimeZoneInfo.FindSystemTimeZoneById("Asia/Jakarta");
    private static TimeZoneInfo WITA = TimeZoneInfo.FindSystemTimeZoneById("Asia/Makassar");
    private static TimeZoneInfo WIT = TimeZoneInfo.FindSystemTimeZoneById("Asia/Jayapura");


    public static DateTime ConvertToWIBTime(DateTime utcTime)
    {
        return TimeZoneInfo.ConvertTimeFromUtc(utcTime, WIB);
    }
    public static DateTime ConvertToWITATime(DateTime utcTime)
    {
        return TimeZoneInfo.ConvertTimeFromUtc(utcTime, WITA);
    }
    public static DateTime ConvertToWITTime(DateTime utcTime)
    {
        return TimeZoneInfo.ConvertTimeFromUtc(utcTime, WIT);
    }
}