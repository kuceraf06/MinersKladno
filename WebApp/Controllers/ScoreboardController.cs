using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Miners.Web.BusinessLayer;
using Miners.Web.BusinessLayer.Entities;

namespace Miners.Web.WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ScoreboardController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var scoreboard = await db.Scores.AsNoTracking().FirstOrDefaultAsync();
        if (scoreboard is null)
        {
            return Ok(new ScoreboardDto(false, "M", string.Empty, string.Empty, string.Empty));
        }

        return Ok(new ScoreboardDto(
            IsEnabled(scoreboard.Active),
            NormalizeTyp(scoreboard.Typ),
            scoreboard.HomeTeam ?? string.Empty,
            scoreboard.VisitorTeam ?? string.Empty,
            scoreboard.MbId ?? string.Empty));
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateScoreboardDto dto)
    {
        var active = ToFlag(dto.Active);
        var typ = NormalizeTyp(dto.Typ);
        if (typ is not ("M" or "B"))
        {
            return BadRequest("Typ musí být M nebo B.");
        }

        var homeTeam = NullIfWhiteSpace(dto.HomeTeam);
        var visitorTeam = NullIfWhiteSpace(dto.VisitorTeam);
        var mbId = typ == "B" ? NullIfWhiteSpace(dto.MbId) : null;

        var scoreboard = await db.Scores.FirstOrDefaultAsync();
        if (scoreboard is null)
        {
            scoreboard = new Scoreboard();
            db.Scores.Add(scoreboard);
        }

        scoreboard.Active = active;
        scoreboard.Typ = typ;
        scoreboard.HomeTeam = homeTeam;
        scoreboard.VisitorTeam = visitorTeam;
        scoreboard.MbId = mbId;

        db.SaveAndCommit();

        return NoContent();
    }

    private static bool IsEnabled(string? value) => string.Equals(value, "Y", StringComparison.OrdinalIgnoreCase);

    private static string ToFlag(bool enabled) => enabled ? "Y" : "N";

    private static string NormalizeTyp(string? typ)
    {
        if (string.IsNullOrWhiteSpace(typ)) return "M";
        return typ.Trim().ToUpperInvariant();
    }

    private static string? NullIfWhiteSpace(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return null;
        return value.Trim();
    }
}

public record ScoreboardDto(bool Active, string Typ, string HomeTeam, string VisitorTeam, string MbId);
public record UpdateScoreboardDto(bool Active, string Typ, string? HomeTeam, string? VisitorTeam, string? MbId);


