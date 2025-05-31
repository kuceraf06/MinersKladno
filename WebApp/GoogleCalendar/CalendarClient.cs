using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;

namespace Miners.Web.WebApp.GoogleCalendar;

public class CalendarClient
{
    // If modifying these scopes, delete your previously saved credentials
    // at ~/.credentials/calendar-dotnet-quickstart.json
    static string[] Scopes = { CalendarService.Scope.CalendarReadonly };
    static string ApplicationName = "Google Calendar API .NET Quickstart";

    public Events Main(DateTime startDate, DateTime endDate)
    {
        // Create Google Calendar API service.
        var service = new CalendarService(new BaseClientService.Initializer()
        {
            ApiKey = "AIzaSyArm-QCSQ3yjE32rOnVw-83o0xhaCfbIxQ",
        });

        // Define parameters of request.
        EventsResource.ListRequest request = service.Events.List("support@minerskladno.cz");
        request.TimeMin = startDate;
        request.TimeMax = endDate.AddDays(1).AddMinutes(-1);
        request.ShowDeleted = false;
        request.SingleEvents = false;
        //request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

        // List events.
        Events events = request.Execute();
        foreach (var ev in events.Items)
        {
            if (ev.Start.DateTime == null)
            {
                ev.Start.DateTime = DateTime.Parse(ev.Start.Date);
            }

            if (ev.End != null && ev.End.DateTime == null)
            {
                ev.End.DateTime = DateTime.Parse(ev.End.Date).AddDays(-1);
            }
        }
        return events;
    }
}