using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using api.CLients;
using Google.Apis.Calendar.v3.Data;

namespace api.Controllers
{

    [ApiController]
    [Route("api/calendar")]
    public class EventCompletionController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EventCompletionController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("complete/{calendarId}/{eventId}")]
        [Authorize]
        public async Task<IActionResult> CompleteEvent(string calendarId, string eventId)
        {
            var calendarClient = new CalendarClient(_httpContextAccessor.HttpContext);
            Event existingEvent = await calendarClient.UpdateEventTime(calendarId, eventId, DateTime.MinValue, DateTime.MinValue);

            // Позначити подію як виконану
            existingEvent.Status = "completed";

            Event updatedEvent = await calendarClient.UpdateEventTime(calendarId, eventId, existingEvent.Start.DateTime.Value, existingEvent.End.DateTime.Value);
            return Ok($"Подія з ID: {eventId} позначена як виконана.");
        }
    }

}
