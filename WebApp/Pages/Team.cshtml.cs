using System.Text.Json;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Miners.Web.BusinessLayer;
using Miners.Web.WebApp.Models;

namespace Miners.Web.WebApp.Pages;

public class Team(AppDbContext dbContext) : PageModel
{
    public TeamModel TeamModel { get; set; }
    public void OnGet(string id)
    {
        id = id.ToLower();
        var teamEntity = dbContext.Teams.Single(x => x.Id == id);
        TeamModel = new TeamModel()
        {
            Title = teamEntity.Title,
            Description = teamEntity.Description,
            Fee = teamEntity.Fee,
            League = teamEntity.League,
            TeamPhoto = teamEntity.TeamPhoto,
        };
        if (!string.IsNullOrEmpty(teamEntity.TrainingDays))
        {
           TeamModel.TrainingDays = JsonSerializer.Deserialize<TrainingDays>(teamEntity.TrainingDays, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }
        Coach(teamEntity.Coach1Id, teamEntity.Coach1Function);
        Coach(teamEntity.Coach2Id, teamEntity.Coach2Function);
        Coach(teamEntity.Coach3Id, teamEntity.Coach3Function);
        Coach(teamEntity.Coach4Id, teamEntity.Coach4Function);
    }

    private void Coach(Guid? guid, string? function)
    {
        if (guid.HasValue)
        {
            var p = dbContext.Persons.Single(x=>x.Id == guid.Value);
            TeamModel.Coaches.Add(new TeamCoach()
            {
                FullName = p.Jmeno + " " + p.Prijmeni,
                Function = function,
                Mobile = p.Telefon,
                Email = p.Email,
                Licence = p.Licence
            });
        }
    }
}