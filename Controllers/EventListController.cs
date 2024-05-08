using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using api.CLients;

namespace api.Controllers
{

    [ApiController]
    [Route("api/calendar")]
    public class EventListController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EventListController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("list/{calendarId}")]
        [Authorize]
        public async Task<IActionResult> GetEventsByTimePeriod(string calendarId, DateTime startTime, DateTime endTime)
        {
            var calendarClient = new CalendarClient(_httpContextAccessor.HttpContext);
            var events = await calendarClient.GetEventsByTimePeriod(calendarId, startTime, endTime);
            return Ok(events);
        }
    }

}
