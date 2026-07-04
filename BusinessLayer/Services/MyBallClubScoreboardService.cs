using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text.Json;
using Miners.Web.BusinessLayer.Entities;

namespace Miners.Web.BusinessLayer.Services;

public class MyBallClubScoreboardService
{
    public static ConcurrentDictionary<string, GameStatus> Latest = new();

    private static readonly HttpClient _client = new();

    private static async Task<string> FetchAsync(string url)
    {
        if (OperatingSystem.IsMacOS())
        {
            // macOS 16 Tahoe: .NET's Interop+AppleCrypto fails TLS 1.3 handshake, curl works fine
            using var process = new Process
            {
                StartInfo = new ProcessStartInfo("curl")
                {
                    ArgumentList = { "-s", "--max-time", "10", url },
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                }
            };
            process.Start();
            var output = await process.StandardOutput.ReadToEndAsync();
            await process.WaitForExitAsync();
            return output;
        }

        return await _client.GetStringAsync(url);
    }

    // Vytáhne game ID z URL, např. ".../box-score/196991" → "196991"
    private static string ExtractGameId(string url)
    {
        var uri = new Uri(url);
        return uri.Segments.Last().TrimEnd('/');
    }

    public async Task<GameStatus> GetGameStatus(Scoreboard scoreboard)
    {
        try
        {
            var gameId = scoreboard.MbId;
            var gameLatest = await FetchAsync($"https://game.wbsc.org/gamedata/{gameId}/latest.json");
            if (Latest.ContainsKey(gameId) && Latest[gameId].Latest == gameLatest)
            {
                return Latest[gameId];
            }

            if (!Latest.ContainsKey(gameId) || Latest[gameId].Latest != gameLatest)
            {
                var jsonDoc = JsonDocument.Parse(await FetchAsync($"https://game.wbsc.org/gamedata/{gameId}/play{gameLatest}.json"));
                var situation =
                    JsonSerializer.Deserialize<MyBallClubGame>(jsonDoc.RootElement.GetProperty("situation"));
                var gameStatus = new GameStatus()
                {
                    Latest = gameLatest,
                    HomeTeam = scoreboard.HomeTeam,
                    VisitorTeam = scoreboard.VisitorTeam,
                    Outs = GetInt(situation.outs),
                    Balls = situation.balls,
                    Strikes = situation.strikes,
                    FirstBase = GetInt(situation.runner1) > 0,
                    SecondBase = GetInt(situation.runner2) > 0,
                    ThirdBase = GetInt(situation.runner3) > 0
                };
                var inningSplit = situation.inning.Split('.');
                gameStatus.Inning = int.Parse(inningSplit[0]);
                gameStatus.Bottom = inningSplit[1] == "1";
                // skore
                gameStatus.VisitorScore = jsonDoc.RootElement.GetProperty("linescore").GetProperty("awaytotals")
                    .GetProperty("R").GetInt32();
                gameStatus.HomeScore = jsonDoc.RootElement.GetProperty("linescore").GetProperty("hometotals")
                    .GetProperty("R").GetInt32();
                // mezi směnami
                var platecount = jsonDoc.RootElement.GetProperty("platecount");
                if (platecount.GetArrayLength() > 0)
                {
                    var pc = platecount[0];
                    gameStatus.BetweenInnings = pc.TryGetProperty("type", out var typeEl) && typeEl.GetInt32() == 0;
                }

                // nadhazovac a palkar
                gameStatus.Pitcher = CapitalizeFirst(situation.pitcher.Split(' ')[0]);
                if (!string.IsNullOrEmpty(situation.batter))
                {
                    gameStatus.Batter = CapitalizeFirst(situation.batter.Split(' ')[0]);
                    gameStatus.BatterLine = situation.batting;
                    gameStatus.BatterAvg = situation.avg;
                }

                var boxscore = jsonDoc.RootElement.GetProperty("boxscore");
                (gameStatus.PitchCount, gameStatus.PitcherStrikes) =
                    FindPitcherInBoxscore(boxscore, situation.pitcherid);
                // linescore
                var linescore = jsonDoc.RootElement.GetProperty("linescore");
                gameStatus.AwayInnings = ParseInnings(linescore.GetProperty("awayruns"));
                gameStatus.HomeInnings = ParseInnings(linescore.GetProperty("homeruns"));
                var awaytotals = linescore.GetProperty("awaytotals");
                gameStatus.AwayHits = awaytotals.GetProperty("H").GetInt32();
                gameStatus.AwayErrors = awaytotals.GetProperty("E").GetInt32();
                gameStatus.AwayLob = awaytotals.GetProperty("LOB").GetInt32();
                var hometotals = linescore.GetProperty("hometotals");
                gameStatus.HomeHits = hometotals.GetProperty("H").GetInt32();
                gameStatus.HomeErrors = hometotals.GetProperty("E").GetInt32();
                gameStatus.HomeLob = hometotals.GetProperty("LOB").GetInt32();
                Latest[gameId] = gameStatus;
                return gameStatus;
            }
        }
        catch (Exception ex)
        {
            return new GameStatus { TeamInfoError = "GetGameStatus: " + ex.GetType().Name + ": " + ex.Message };
        }

        return null;
    }

    private int GetInt(JsonElement element)
    {
        if (element.ValueKind == JsonValueKind.String)
            return int.Parse(element.GetString());
        if (element.ValueKind == JsonValueKind.Number)
            return element.GetInt32();
        return 0;
    }

    private string CapitalizeFirst(string text)
    {
        if (string.IsNullOrEmpty(text)) return text;
        return char.ToUpper(text[0]) + text.Substring(1).ToLower();
    }

    private string?[] ParseInnings(JsonElement runsArray)
    {
        var len = runsArray.GetArrayLength();
        var result = new string?[len - 1];
        for (int i = 1; i < len; i++)
        {
            var el = runsArray[i];
            result[i - 1] = el.ValueKind == JsonValueKind.Null
                ? null
                : (el.ValueKind == JsonValueKind.Number ? el.GetInt32().ToString() : el.GetString());
        }

        return result;
    }

    private (int? pitches, int? so) FindPitcherInBoxscore(JsonElement boxscore, string pitcherId)
    {
        foreach (var player in boxscore.EnumerateObject())
        {
            var playerData = player.Value;
            if (playerData.GetProperty("playerid").GetString() != pitcherId) continue;
            int? pitches = playerData.TryGetProperty("PITCHES", out var p) ? p.GetInt32() : null;
            int? strikes = playerData.TryGetProperty("STRIKES", out var s) ? s.GetInt32() : null;
            return (pitches, strikes);
        }

        return (null, null);
    }
}