using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using api.CLients;

namespace api.Controllers
{

    [ApiController]
    [Route("api/calendar")]
    public class EventDeletionController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EventDeletionController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpDelete("delete/{calendarId}/{eventId}")]
        [Authorize]
        public async Task<IActionResult> DeleteEvent(string calendarId, string eventId)
        {
            var calendarClient = new CalendarClient(_httpContextAccessor.HttpContext);
            await calendarClient.DeleteEvent(calendarId, eventId);
            return Ok($"Подія з ID: {eventId} видалена.");
        }
    }

}
