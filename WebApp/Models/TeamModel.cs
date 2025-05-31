using System.ComponentModel.DataAnnotations.Schema;

namespace Miners.Web.WebApp.Models;

public class TeamModel
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public TrainingDays? TrainingDays { get; set; }
    public string? Fee { get; set; }
    public string? League { get; set; }
    public Guid? TeamPhoto { get; set; }
    public List<TeamCoach> Coaches { get; set; } = new();

}

public class TrainingDays
{
    public List<TrainingDay> Leto { get; set; }
    public List<TrainingDay> Zima { get; set; }
}

public class TrainingDay
{
    public string Den { get; set; }
    public string Cas { get; set; }
    public string Misto { get; set; }
    public string Poznamka { get; set; }
}

public class TeamCoach
{
    public string FullName { get; set; }
    public string Function { get; set; }
    public string Mobile { get; set; }
    public string Email { get; set; }
    public string Licence { get; set; }
}