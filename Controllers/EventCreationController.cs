using api.CLients;
using api.Models;
using Google.Apis.Calendar.v3.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/calendar")]
    public class EventCreationController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EventCreationController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreateEvent([FromBody] EventData eventData)
        {
            var calendarClient = new CalendarClient(_httpContextAccessor.HttpContext);
            Event createdEvent = await calendarClient.CreateEvent(eventData.CalendarId, eventData.Summary, eventData.StartTime, eventData.EndTime);
            return Ok($"Подія '{createdEvent.Summary}' створена з ID: {createdEvent.Id}");
        }
    }
}
