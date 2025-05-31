using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Miners.Web.BusinessLayer;
using Miners.Web.WebApp.Models;

namespace Miners.Web.WebApp.Pages;

public class Clanek(AppDbContext dbContext) : PageModel
{
    public ClanekModel ClanekModel { get; set; }
    public void OnGet(string id)
    {
        id = id.Split('-').First();
        var article = dbContext.Articles.Single(x => x.UrlId == int.Parse(id));
        ClanekModel = new ClanekModel()
        {
            Title = article.Title,
            Text = article.Text,
        };
    }
}