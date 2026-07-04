namespace Miners.Web.WebApp.Models;

public class GameModel
{
    public string Category { get; set; }
    public DateOnly GameDate { get; set; }
    public string League { get; set; }
    public string HomeTeam { get; set; }
    public string AwayTeam { get; set; }
    public int? HomeScore { get; set; }
    public int? AwayScore { get; set; }
    
}