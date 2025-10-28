using BusinessLayer.Entities;

namespace Miners.Web.BusinessLayer.Entities;

public class Game : GuidEntityBase
{
    public required Guid CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    
    public required DateOnly GameDate { get; set; }
    public required string League { get; set; }
    public required string HomeTeam { get; set; }
    public required string AwayTeam { get; set; }
    public int? HomeScore { get; set; }
    public int? AwayScore { get; set; }
}