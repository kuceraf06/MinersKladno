using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Miners.Web.BusinessLayer;
using Miners.Web.BusinessLayer.Services;
using Newtonsoft.Json.Linq;

namespace Miners.Web.WebApp.Pages;

public class Scoreboard(AppDbContext dbContext) : PageModel
{
        public bool Active { get; private set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnGetUpdateAsync()
        {
            var scoreboard = dbContext.Scores.First();
            if (scoreboard.Active != "Y") return new JsonResult(null);

            if (scoreboard.Typ == "M")
            {
                var gs = new GameStatus(scoreboard);
                gs.Type = "M";
                return new JsonResult(gs);
            }

            if (scoreboard.Typ == "B")
            {
                var mb = new MyBallClubScoreboardService();
                var gs = await mb.GetGameStatus(scoreboard);
                gs.Type = "B";
                return new JsonResult(gs);
            }
            return new JsonResult(null);
        }

        private string GetTeamName(string token)
        {
            var htt = token.Split(":");
            var home = WebUtility.UrlDecode(htt[2]).Split(' ')[0];
            home = home.Substring(0, Math.Min(home.Length, 7));
            return home.ToUpper();
        }

        private string GetUrl(string url)
        {
            var uri = new Uri(url);
            return "http://dc.iscorecast.com/gamesync.jsp" + uri.Query;
        }
    }