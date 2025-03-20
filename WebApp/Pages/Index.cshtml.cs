using BusinessLayer.Entities;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Miners.Web.BusinessLayer;
using Miners.Web.WebApp.GoogleCalendar;
using Miners.Web.WebApp.Models;

namespace Miners.Web.WebApp.Pages;

public class IndexModel(AppDbContext dbContext) : PageModel
{
    public List<CarouselArticleModel> CarouselArticles { get; set; }
    public CalendarModel Calendar { get; set; }
    
    public void OnGet()
    {
        CarouselArticles = dbContext.Articles.OrderByDescending(x => x.DateCreated).Take(3).Select(x => new CarouselArticleModel
        {
            Id = x.Id,
            ImageId = x.GuidImage,
            Title = x.Title,
            DateCreated = x.DateCreated,
            Teaser = x.Text
        }).ToList();
        CarouselArticles.ForEach(x=>x.Teaser = CreateTeaser(x.Teaser));
        CreateCalendar(DateTime.Now);
    }
    
    private string CreateTeaser(string htmlContent)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(htmlContent);
        var plainText = doc.DocumentNode.InnerText;
        return plainText.Length > 200 ? plainText.Substring(0, 200) + "..." : plainText;
    }
    
    private void CreateCalendar(DateTime now)
        {
            
            var firstOfMonth = now.AddDays(1 - now.Day);
            var lastOfMonth = firstOfMonth.AddMonths(1).AddDays(-1);
            var startDate = GetFirstDayOfWeek(firstOfMonth);
            var endDate = lastOfMonth.AddDays(7 - (double)lastOfMonth.DayOfWeek);
            CalendarClient cc = new CalendarClient();
            var events = cc.Main(startDate, endDate).Items;
            //var events = _dbContext.Events.Where(x => startDate <= (x.DateTo ?? x.DateFrom) && endDate >= x.DateFrom).ToList();
            Calendar = new CalendarModel
            {
                Month = now.ToString("MMMM") + " " + now.Year.ToString(),
                PrevMonth = now.AddMonths(-1).Month.ToString().PadLeft(2, '0') + now.AddMonths(-1).Year.ToString(),
                NextMonth = now.AddMonths(1).Month.ToString().PadLeft(2, '0') + now.AddMonths(1).Year.ToString(),
                Weeks = new List<Week>()
            };
            Week currentWeek = null;
            var realNow = DateTime.Now;
            for (var i = startDate; i <= endDate; i = i.AddDays(1))
            {
                if (i.DayOfWeek == DayOfWeek.Monday)
                {
                    currentWeek = new Week { Days = new List<Day>() };
                    Calendar.Weeks.Add(currentWeek);
                }
                var day = new Day
                {
                    Date = i,
                };
                if(events.Any(x=> i.Date>= x.Start.DateTime.Value.Date && i.Date<= (x.End.DateTime?.Date ?? x.Start.DateTime.Value.Date)))
                //if (events.Any(x=> i>= x.DateFrom && i<= (x.DateTo ?? x.DateFrom)))
                {
                    day.CssClass = "event";
                    day.IsEvent = true;
                }
                else if (i.Month != now.Month)
                {
                    day.CssClass = "inactive";
                }
                if(day.Date == realNow.Date)
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