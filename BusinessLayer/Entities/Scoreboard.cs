using System.ComponentModel.DataAnnotations.Schema;
using BusinessLayer.Entities;

namespace Miners.Web.BusinessLayer.Entities;

[Table("tbl_scoreboard", Schema = "sch_miners")]
public class Scoreboard : GuidEntityBase
{
    public string Active { get; set; } = "N";
    public string Typ { get; set; } = "M";
    public string? HomeTeam { get; set; }
    public string? VisitorTeam { get; set; }
    public int HomeScore { get; set; } = 0;
    public int VisitorScore { get; set; } = 0;
    public int Inning { get; set; } = 0;
    public bool TopInning { get; set; } = false;
    public int Outs { get; set; } = 0;
    public string? MbId { get; set; }
    public string LiveTv { get; set; } = "N";
    public string LiveTvUrl { get; set; } = string.Empty;
}