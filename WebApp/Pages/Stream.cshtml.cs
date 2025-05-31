using Microsoft.AspNetCore.Mvc.RazorPages;
using Miners.Web.BusinessLayer;

namespace Miners.Web.WebApp.Pages;

public class Stream(AppDbContext dbContext) : PageModel
{
    public string LiveTv { get; set; } = "";
    public string LiveTvUrl { get; set; } = "";
    
    public void OnGet()
    {
        var scoreboard = dbContext.Scores.First();
        LiveTv = scoreboard.LiveTv;
        
        LiveTvUrl = CreateEmbedUrlFromYouTubeUrl(scoreboard.LiveTvUrl);
    }
    
    private static string CreateEmbedUrlFromYouTubeUrl(string url)
    {
        if (string.IsNullOrEmpty(url))
            return string.Empty;

        string videoId;
        
        if (url.Contains("youtu.be/"))
        {
            videoId = url.Split("youtu.be/").Last().Split('?').First();
        }
        else if (url.Contains("/live/"))
        {
            videoId = url.Split("/live/").Last().Split('?').First();
        }
        else if (url.Contains("youtube.com"))
        {
            var uri = new Uri(url);
            var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
            videoId = query["v"] ?? string.Empty;
        }
        else
        {
            return string.Empty;
        }

        return string.IsNullOrEmpty(videoId) 
            ? string.Empty 
            : $"https://www.youtube.com/embed/{videoId}";
    }


}