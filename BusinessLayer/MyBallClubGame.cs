using System.Text.Json;

namespace Miners.Web.BusinessLayer;

public class MyBallClubGame
{
    public string latest { get; set; }    
    //public string gameid { get; set; }
    public string inning { get; set; }
    public int balls { get; set; }
    public int strikes { get; set; }
    //public JsonElement homeruns { get; set; }
    //public JsonElement awayruns { get; set; }
    public JsonElement outs { get; set; }
    public JsonElement runner1 { get; set; }
    public JsonElement runner2 { get; set; }
    public JsonElement runner3 { get; set; }
    public string pitcherid { get; set; }
    public string pitcher { get; set; }
    public string batterid { get; set; }
    public string batter { get; set; }
    public string batting { get; set; }
    public string avg { get; set; }
}