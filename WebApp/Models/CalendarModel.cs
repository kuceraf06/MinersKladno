namespace Miners.Web.WebApp.Models;

public class CalendarModel
{
    public string Month { get; set; }
    public string Year { get; set; }
    public List<Week> Weeks { get; set; }
    public string PrevMonth { get; internal set; }
    public string NextMonth { get; internal set; }
}
public class Day
{
    public DateTime Date { get; internal set; }
    public bool Inactive { get; internal set; }
    public string CssClass { get; internal set; }
    public bool IsEvent { get; internal set; }
}

public class Week
{
    public List<Day> Days { get; set; }
}

public class EventsModel
{
    public DateTime Date { get; set; }
    public List<EventModel> Events{ get; set; }
}

public class EventModel
{
    public DateTime Start { get; set; }
    public DateTime? End { get; set; }
    public string Text { get; set; }
    public string Location { get; set; }
    public string Date { get; internal set; }
}