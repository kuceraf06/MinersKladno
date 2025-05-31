using System.ComponentModel.DataAnnotations.Schema;
using BusinessLayer.Entities;

namespace Miners.Web.BusinessLayer.Entities;

[Table("tbl_scoreboard", Schema = "sch_miners")]
public class Scoreboard
{
    public string LiveTv { get; set; }
    public string LiveTvUrl { get; set; }
}