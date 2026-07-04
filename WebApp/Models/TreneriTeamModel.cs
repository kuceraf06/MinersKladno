namespace Miners.Web.WebApp.Models;

public class TreneriTeamModel
{
    public string Title { get; set; }
    public List<TrenerModel> Treneri { get; set; } = new List<TrenerModel>();
    public string Kategorie { get; set; }
}