using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace api.CLients
{

    public class CalendarClient
    {
        private readonly HttpContext _context;
        private CalendarService _service;

        public CalendarClient(HttpContext context)
        {
            _context = context;
            InitializeCalendarService();
        }

        private void InitializeCalendarService()
        {
            var accessToken = _context.User.FindFirst("access_token")?.Value;

            if (string.IsNullOrEmpty(accessToken))
            {
                throw new InvalidOperationException("Access token not found in HttpContext");
            }

            var credential = GoogleCredential.FromAccessToken(accessToken);

            _service = new CalendarService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = Constants.ApplicationName
            });
        }

        public async Task<Event> CreateEvent(string calendarId, string summary, DateTime startTime, DateTime endTime)
        {
            Event newEvent = new Event
            {
                Summary = summary,
                Start = new EventDateTime { DateTime = startTime },
                End = new EventDateTime { DateTime = endTime }
            };

            Event createdEvent = await _service.Events.Insert(newEvent, calendarId).ExecuteAsync();
            return createdEvent;
        }

        public async Task<bool> DeleteEvent(string calendarId, string eventId)
        {
            await _service.Events.Delete(calendarId, eventId).ExecuteAsync();
            return true;
        }

        public async Task<Event> UpdateEventTime(string calendarId, string eventId, DateTime newStartTime, DateTime newEndTime)
        {
            Event existingEvent = await _service.Events.Get(calendarId, eventId).ExecuteAsync();

            existingEvent.Start.DateTime = newStartTime;
            existingEvent.End.DateTime = newEndTime;

            Event updatedEvent = await _service.Events.Update(existingEvent, calendarId, eventId).ExecuteAsync();
            return updatedEvent;
        }
        public async Task<IList<Event>> GetEventsByTimePeriod(string calendarId, DateTime startTime, DateTime endTime)
        {
            var request = _service.Events.List(calendarId);
            request.TimeMin = startTime.ToUniversalTime();
            request.TimeMax = endTime.ToUniversalTime();
            Events eventsResponse = await request.ExecuteAsync();
            return eventsResponse.Items;
        }
    }
}
