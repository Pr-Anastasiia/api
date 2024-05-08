using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using api.CLients;
using Google.Apis.Calendar.v3.Data;
using api.Models;

namespace api.Controllers
{

    [ApiController]
    [Route("api/calendar")]
    public class EventUpdateController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EventUpdateController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPut("update/{calendarId}/{eventId}")]
        [Authorize]
        public async Task<IActionResult> UpdateEventTime(string calendarId, string eventId, [FromBody] EventData eventData)
        {
            var calendarClient = new CalendarClient(_httpContextAccessor.HttpContext);
            Event updatedEvent = await calendarClient.UpdateEventTime(calendarId, eventId, eventData.StartTime, eventData.EndTime);
            return Ok($"Час події з ID: {eventId} оновлений.");
        }
    }

}
