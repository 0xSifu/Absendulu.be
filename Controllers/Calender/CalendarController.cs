using AbsenDulu.BE.Interfaces.IServices.Calender;
using AbsenDulu.BE.Models.Calender;
using AbsenDulu.BE.Models.CalenderDay;
using AbsenDulu.BE.Response;
using Microsoft.AspNetCore.Mvc;


namespace AbsenDulu.BE.Controllers.Calender;
[ApiController]
public class CalendarController : ControllerBase
{
    private readonly IGenerateCalenderServices _calendarService;

    public CalendarController(IGenerateCalenderServices calendarService)
    {
        _calendarService = calendarService;
    }

    [HttpPost]
    [Route("[controller]/generate")]

    public ActionResult<List<CalenderDay>> GenerateCalendar([FromBody] CalendarRequest request)
    {
        int year = request.Year;
        List<Holiday> holidays = request.Holidays;
        var calendar = _calendarService.GenerateCalendar(year,holidays);
        return Ok(new ResponseMessage<List<CalenderDay>> { IsError = false, Message = "Success", Data = calendar });
    }

    [HttpGet]
    [Route("[controller]/GetCalendar")]

    public ActionResult<List<CalenderDay>> GetCalendar([FromQuery] int year,int month)
    {
        var calendar = _calendarService.GetCalendar(year,month);
        return Ok(new ResponseMessage<List<CalenderDay>> { IsError = false, Message = "Success", Data = calendar });
    }
}
