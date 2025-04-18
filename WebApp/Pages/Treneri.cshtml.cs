using Microsoft.AspNetCore.Mvc.RazorPages;
using Miners.Web.BusinessLayer;
using Miners.Web.BusinessLayer.Entities;
using Miners.Web.WebApp.Models;

namespace Miners.Web.WebApp.Pages;

public class Treneri(AppDbContext dbContext) : PageModel
{
    public List<TreneriTeamModel> Teams { get; set; } = new List<TreneriTeamModel>();
    public void OnGet()
    {
        var teams = dbContext.Teams.OrderBy(x=>x.Order).ToList();
        var persons = dbContext.Persons.ToList();
        foreach (var team in teams)
        {
            var teamModel = new TreneriTeamModel
            {
                Title = team.Title,
                Kategorie = team.TreneriKategorie
            };
            Trener(persons, team.Coach1Id, teamModel);
            Trener(persons, team.Coach2Id, teamModel);
            Trener(persons, team.Coach3Id, teamModel);
            Trener(persons, team.Coach4Id, teamModel);
            Teams.Add(teamModel);
        }
    }
    
    private static void Trener(IEnumerable<Person> persons, Guid? guidCoach, TreneriTeamModel teamModel)
    {
        if (guidCoach.HasValue)
        {
            var person = persons.SingleOrDefault(x => x.Id == guidCoach);
            if (person != null)
            {
                teamModel.Treneri.Add(new TrenerModel(person));
            }
        }
    }
}