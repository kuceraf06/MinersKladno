using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Miners.Web.BusinessLayer;
using Miners.Web.BusinessLayer.Entities;

namespace Miners.Web.WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GamesController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(int page = 1, int pageSize = 20, string sortField = "gameDate", string sortOrder = "desc")
    {
        var query = db.Games.Include(g => g.Category).AsQueryable();

        query = (sortField, sortOrder) switch
        {
            ("gameDate", "asc")       => query.OrderBy(g => g.GameDate),
            ("league", "asc")         => query.OrderBy(g => g.League),
            ("categoryTitle", "asc")  => query.OrderBy(g => g.Category.Title),
            ("league", "desc")        => query.OrderByDescending(g => g.League),
            ("categoryTitle", "desc") => query.OrderByDescending(g => g.Category.Title),
            _                         => query.OrderByDescending(g => g.GameDate),
        };

        var total = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(g => new GameDto(
                g.Id, g.CategoryId, g.Category.Title, g.Category.Code,
                g.GameDate, g.League, g.HomeTeam, g.AwayTeam, g.HomeScore, g.AwayScore))
            .ToListAsync();

        return Ok(new { total, items });
    }

    [HttpPost]
    public IActionResult Create([FromBody] GameUpsertDto dto)
    {
        var game = new Game
        {
            CategoryId = dto.CategoryId,
            GameDate = dto.GameDate,
            League = dto.League,
            HomeTeam = dto.HomeTeam,
            AwayTeam = dto.AwayTeam,
            HomeScore = dto.HomeScore,
            AwayScore = dto.AwayScore,
        };
        db.Games.Add(game);
        db.SaveAndCommit();
        return Ok(game.Id);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] GameUpsertDto dto)
    {
        var game = await db.Games.FindAsync(id);
        if (game is null) return NotFound();

        game.CategoryId = dto.CategoryId;
        game.GameDate = dto.GameDate;
        game.League = dto.League;
        game.HomeTeam = dto.HomeTeam;
        game.AwayTeam = dto.AwayTeam;
        game.HomeScore = dto.HomeScore;
        game.AwayScore = dto.AwayScore;

        db.SaveAndCommit();
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var game = await db.Games.FindAsync(id);
        if (game is null) return NotFound();
        db.Games.Remove(game);
        db.SaveAndCommit();
        return NoContent();
    }
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CategoriesController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var cats = await db.Categories
            .OrderBy(c => c.Title)
            .Select(c => new { c.Id, c.Code, c.Title })
            .ToListAsync();
        return Ok(cats);
    }
}

public record GameDto(
    Guid Id, Guid CategoryId, string CategoryTitle, string CategoryCode,
    DateOnly GameDate, string League, string HomeTeam, string AwayTeam,
    int? HomeScore, int? AwayScore);

public record GameUpsertDto(
    Guid CategoryId, DateOnly GameDate, string League,
    string HomeTeam, string AwayTeam, int? HomeScore, int? AwayScore);
