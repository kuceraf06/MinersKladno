using Miners.Web.BusinessLayer.Entities;

namespace Miners.Web.WebApp.Models;

public class TrenerModel(Person person)
{
    public string FullName { get; set; } = person.Jmeno + " " + person.Prijmeni;
    public string Function { get; set; } = person.PoziceTym;
    public string Email { get; set; } = person.Email;
    public string Mobile { get; set; } = person.Telefon;
    public string Licence { get; set; } = person.Licence;
    public string? Foto { get; set; }
}