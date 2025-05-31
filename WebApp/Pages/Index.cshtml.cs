using BusinessLayer.Entities;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Miners.Web.BusinessLayer;
using Miners.Web.WebApp.GoogleCalendar;
using Miners.Web.WebApp.Models;

namespace Miners.Web.WebApp.Pages;

public class IndexModel(AppDbContext dbContext) : PageModel
{
    public List<CarouselArticleModel> CarouselArticles { get; set; }
    public CalendarModel Calendar { get; set; }

    public void OnGet([FromQuery] string? month)
    {
        LoadArticles();

        DateTime targetDate = GetTargetDate(month);
        CreateCalendar(targetDate);
    }

    public PartialViewResult OnGetCalendarPartial(string? month)
    {
        DateTime targetDate = GetTargetDate(month);
        CreateCalendar(targetDate);

        return new PartialViewResult
        {
            ViewName = "_Calendar", 
            ViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<CalendarModel>(ViewData, Calendar)
        };
    }

    private void LoadArticles()
    {
        CarouselArticles = dbContext.Articles
            .OrderByDescending(x => x.DateCreated)
            .Take(3)
            .Select(x => new CarouselArticleModel
            {
                Id = x.Id,
                ImageId = x.GuidImage,
                Title = x.Title,
                DateCreated = x.DateCreated,
                Teaser = x.Text
            }).ToList();

        CarouselArticles.ForEach(x => x.Teaser = CreateTeaser(x.Teaser));
    }

    private DateTime GetTargetDate(string? month)
    {
        if (!string.IsNullOrEmpty(month) &&
            DateTime.TryParseExact(month, "yyyy-MM", null, System.Globalization.DateTimeStyles.None, out var parsedDate))
        {
            return new DateTime(parsedDate.Year, parsedDate.Month, 1);
        }

        return DateTime.Now;
    }

    private string CreateTeaser(string htmlContent)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(htmlContent);
        var plainText = doc.DocumentNode.InnerText;
        return plainText.Length > 200 ? plainText[..200] + "..." : plainText;
    }

    private void CreateCalendar(DateTime now)
    {
        var culture = new System.Globalization.CultureInfo("cs-CZ");
        string monthName = culture.DateTimeFormat.GetMonthName(now.Month);
        string monthYear = $"{char.ToUpper(monthName[0]) + monthName[1..]} {now.Year}";


        var firstOfMonth = new DateTime(now.Year, now.Month, 1);
        var lastOfMonth = firstOfMonth.AddMonths(1).AddDays(-1);
        var startDate = GetFirstDayOfWeek(firstOfMonth);
        var endDate = lastOfMonth.AddDays(6 - (int)lastOfMonth.DayOfWeek);

        CalendarClient cc = new CalendarClient();
        var events = cc.Main(startDate, endDate).Items;

        Calendar = new CalendarModel
        {
            Year = now.Year.ToString(),
            Month = now.Month.ToString(),
            PrevMonth = now.AddMonths(-1).ToString("yyyy-MM"),
            NextMonth = now.AddMonths(1).ToString("yyyy-MM"),
            MonthNameYear = monthYear,
            Weeks = new List<Week>()
        };

        Week currentWeek = null;
        var realNow = DateTime.Now;

        for (var i = startDate; i <= endDate; i = i.AddDays(1))
        {
            if (i.DayOfWeek == DayOfWeek.Monday || currentWeek == null)
            {
                currentWeek = new Week { Days = new List<Day>() };
                Calendar.Weeks.Add(currentWeek);
            }

            var day = new Day
            {
                Date = i,
                CssClass = i.Month != now.Month ? "inactive" : "",
                IsEvent = false
            };

            if (events.Any(x =>
                i.Date >= x.Start.DateTime.Value.Date &&
                i.Date <= (x.End.DateTime?.Date ?? x.Start.DateTime.Value.Date)))
            {
                day.CssClass += " event";
                day.IsEvent = true;
            }

            if (i.Date == realNow.Date)
            {
                day.CssClass += " active";
            }

            currentWeek.Days.Add(day);
        }

        static DateTime GetFirstDayOfWeek(DateTime dayInWeek)
        {
            DayOfWeek firstDay = DayOfWeek.Monday;
            DateTime firstDayInWeek = dayInWeek.Date;
            while (firstDayInWeek.DayOfWeek != firstDay)
                firstDayInWeek = firstDayInWeek.AddDays(-1);

            return firstDayInWeek;
        }
    }
}
