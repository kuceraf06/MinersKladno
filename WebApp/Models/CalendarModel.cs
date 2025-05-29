namespace Miners.Web.WebApp.Models
{
    public class CalendarModel
    {
        public string Month { get; set; }
        public string Year { get; set; }
        public List<Week> Weeks { get; set; }

        public string PrevMonth { get; set; }
        public string NextMonth { get; set; }

        public string MonthNameYear { get; set; }

    }

    public class Day
    {
        public DateTime Date { get; set; }

        public bool Inactive { get; set; }

        public string CssClass { get; set; } = string.Empty;

        public bool IsEvent { get; set; }
    }

    public class Week
    {
        public List<Day> Days { get; set; } = new();
    }

    public class EventsModel
    {
        public DateTime Date { get; set; }
        public List<EventModel> Events { get; set; } = new();
    }

    public class EventModel
    {
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public string Text { get; set; }
        public string Location { get; set; }

        public string DateLabel { get; set; }
    }
}
