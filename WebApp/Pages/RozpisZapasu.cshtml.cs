using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using Miners.Web.BusinessLayer;
using Miners.Web.WebApp.Models;

namespace Miners.Web.WebApp.Pages;

public class RozpisZapasu(AppDbContext dbContext) : PageModel
{
    public void OnGet()
    {
        GamesModel = dbContext.Games.OrderByDescending(x => x.GameDate).Select(x => new GameModel()
        {
            Category = x.Category.Title,
            League = x.League,
            HomeTeam = x.HomeTeam,
            AwayTeam = x.AwayTeam,
            GameDate = x.GameDate,
            HomeScore = x.HomeScore,
            AwayScore = x.AwayScore,
        }).ToList();
    }

    public List<GameModel> GamesModel { get; set; }
}