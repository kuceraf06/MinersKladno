using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Miners.Web.BusinessLayer;
using Miners.Web.BusinessLayer.Services;
using Article = BusinessLayer.Entities.Article;
using DbFile = BusinessLayer.Entities.File;

namespace Miners.Web.WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ArticlesController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(int page = 1, int pageSize = 20, string sortOrder = "desc")
    {
        var query = db.Articles.AsQueryable();
        query = sortOrder == "asc"
            ? query.OrderBy(a => a.DateCreated)
            : query.OrderByDescending(a => a.DateCreated);

        var total = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(a => new ArticleListDto(
                a.Id, a.Title, a.DateCreated, a.Top, a.UrlId,
                a.GuidImage != Guid.Empty))
            .ToListAsync();

        return Ok(new { total, items });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var a = await db.Articles.FindAsync(id);
        if (a is null) return NotFound();
        return Ok(new ArticleDetailDto(a.Id, a.Title, a.Text, a.DateCreated, a.Top, a.UrlId, a.GuidImage));
    }

    [HttpPost]
    public IActionResult Create([FromBody] ArticleUpsertDto dto)
    {
        return BadRequest("Image is required. Use multipart endpoint /api/articles/create.");
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateWithImage([FromForm] string title, [FromForm] string text, [FromForm] bool top, [FromForm] IFormFile? image)
    {
        if (image == null || image.Length == 0)
            return BadRequest("Image is required.");

        var nextUrlId = await db.Articles.MaxAsync(a => (int?)a.UrlId) + 1 ?? 1;
        using var ms = new MemoryStream();
        await image.CopyToAsync(ms);

        var dbFile = new DbFile
        {
            Id = Guid.NewGuid(),
            Data = ms.ToArray(),
            Filename = image.FileName,
            Size = (int)image.Length,
        };
        db.Files.Add(dbFile);

        var article = new Article
        {
            Title = title,
            Text = text,
            Top = top,
            DateCreated = DateTime.UtcNow,
            GuidImage = dbFile.Id,
            Image = dbFile,
            UrlId = nextUrlId,
        };

        db.Articles.Add(article);
        db.SaveAndCommit();
        return Ok(article.Id);
    }

    [HttpPost("inline-image")]
    public async Task<IActionResult> UploadInlineImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Image is required.");

        if (file.ContentType == null || !file.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
            return BadRequest("Only image files are allowed.");

        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);

        var dbFile = new DbFile
        {
            Id = Guid.NewGuid(),
            Data = ms.ToArray(),
            Filename = file.FileName,
            Size = (int)file.Length,
        };

        db.Files.Add(dbFile);
        db.SaveAndCommit();

        return Ok(new { guid = dbFile.Id, url = $"/File?id={dbFile.Id}" });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ArticleUpsertDto dto)
    {
        var article = await db.Articles.FindAsync(id);
        if (article is null) return NotFound();

        article.Title = dto.Title;
        article.Text = dto.Text;
        article.Top = dto.Top;

        db.SaveAndCommit();
        return NoContent();
    }

    [HttpPut("image/{id:guid}")]
    public async Task<IActionResult> UpdateWithImage(Guid id, [FromForm] string title, [FromForm] string text, [FromForm] bool top, [FromForm] IFormFile? image)
    {
        var article = await db.Articles.FindAsync(id);
        if (article is null) return NotFound();

        article.Title = title;
        article.Text = text;
        article.Top = top;

        // Pokud je přidán nový obrázek, nahradíme starý
        if (image != null && image.Length > 0)
        {
            DbFile? existingFile = null;
            if (article.GuidImage != Guid.Empty)
            {
                existingFile = await db.Files.FindAsync(article.GuidImage);
            }

            using var ms = new MemoryStream();
            await image.CopyToAsync(ms);
            var imageData = ms.ToArray();

            if (existingFile != null)
            {
                existingFile.Data = imageData;
                existingFile.Filename = image.FileName;
                existingFile.Size = (int)image.Length;
            }
            else
            {
                var newFile = new DbFile
                {
                    Data = imageData,
                    Filename = image.FileName,
                    Size = (int)image.Length,
                };
                db.Files.Add(newFile);
                article.GuidImage = newFile.Id;
            }
        }

        db.SaveAndCommit();
        return NoContent();
    }

    [HttpPatch("{id:guid}/top")]
    public async Task<IActionResult> SetTop(Guid id, [FromBody] SetTopDto dto)
    {
        var article = await db.Articles.FindAsync(id);
        if (article is null) return NotFound();
        article.Top = dto.Top;
        db.SaveAndCommit();
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var article = await db.Articles.FindAsync(id);
        if (article is null) return NotFound();
        db.Articles.Remove(article);
        db.SaveAndCommit();
        return NoContent();
    }

    [HttpPost("{id:guid}/image")]
    public async Task<IActionResult> UploadImage(Guid id, IFormFile file)
    {
        var article = await db.Articles.FindAsync(id);
        if (article is null) return NotFound();

        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);

        // nahradit stávající obrázek nebo vytvořit nový
        DbFile? existing = null;
        if (article.GuidImage != Guid.Empty)
            existing = await db.Files.FindAsync(article.GuidImage);

        if (existing is not null)
        {
            existing.Data = ms.ToArray();
            existing.Filename = file.FileName;
            existing.Size = (int)file.Length;
        }
        else
        {
            existing = new DbFile
            {
                Data = ms.ToArray(),
                Filename = file.FileName,
                Size = (int)file.Length,
            };
            db.Files.Add(existing);
            db.SaveAndCommit();
            article.GuidImage = existing.Id;
        }

        db.SaveAndCommit();
        return Ok(article.GuidImage);
    }
}

public record ArticleListDto(Guid Id, string? Title, DateTime DateCreated, bool Top, int UrlId, bool HasImage);
public record ArticleDetailDto(Guid Id, string? Title, string Text, DateTime DateCreated, bool Top, int UrlId, Guid GuidImage);
public record ArticleUpsertDto(string Title, string Text, bool Top);
public record SetTopDto(bool Top);
