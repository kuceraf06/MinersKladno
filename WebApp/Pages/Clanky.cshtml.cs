using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Miners.Web.BusinessLayer;
using Miners.Web.BusinessLayer.Services;
using Miners.Web.WebApp.Models;

namespace Miners.Web.WebApp.Pages;

public class Clanky(AppDbContext dbContext) : PageModel
{
    public List<ArticleModel> Articles { get; set; } = new();
    private const int PageSize = 20;
    public bool HasMoreArticles { get; set; }
    public int CurrentPage { get; set; }

    public IActionResult OnGet([FromQuery] int page = 1, [FromQuery] int? year = null, [FromQuery] int? month = null)
    {
        CurrentPage = page;

        var query = dbContext.Articles.AsQueryable();

        if (year.HasValue)
            query = query.Where(x => x.DateCreated.Year == year.Value);

        if (month.HasValue)
            query = query.Where(x => x.DateCreated.Month == month.Value);

        var articles = query.OrderByDescending(x => x.DateCreated)
            .Skip((CurrentPage - 1) * PageSize)
            .Take(PageSize + 1)
            .ToList();

        HasMoreArticles = articles.Count > PageSize;
        if (HasMoreArticles)
        {
            articles.RemoveAt(PageSize);
        }

        Articles = articles.Select(x => new ArticleModel()
        {
            Id = x.Id,
            Date = x.DateCreated.ToShortDateString(),
            Title = x.Title,
            ImageId = x.GuidImage,
            Teaser = ArticleService.CreateTeaser(x.Text),
            Url = ArticleService.CreateUrl(x.Title, x.UrlId)
        }).ToList();

        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            return new PartialViewResult
            {
                ViewName = "_ClankyList",
                ViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<List<ArticleModel>>(ViewData, Articles)
            };
        }

        return Page();
    }

    public async Task<IActionResult> OnGetImageAsync(Guid id)
    {
        var guidImage = await dbContext.Articles
            .Where(x => x.Id == id)
            .Select(x => x.GuidImage)
            .SingleAsync();

        var file = await dbContext.Files
            .SingleOrDefaultAsync(x => x.Id == guidImage);

        return File(file.Data, "image/jpeg");
    }
}