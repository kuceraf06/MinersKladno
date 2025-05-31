using System.ComponentModel.DataAnnotations.Schema;
using BusinessLayer.Entities;

namespace Miners.Web.BusinessLayer.Entities;

[Table("tbl_team", Schema = "sch_miners")]
public class Team : GuidEntityBase
{
    public string Id { get; set; }
    public string Category { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public string? TrainingDays { get; set; }
    public string? Fee { get; set; }
    public string? League { get; set; }
    [Column("coach1_function")]
    public string? Coach1Function { get; set; }
    [Column("coach2_function")]
    public string? Coach2Function { get; set; }
    [Column("coach3_function")]
    public string? Coach3Function { get; set; }
    [Column("coach4_function")]
    public string? Coach4Function { get; set; }
    [Column("guid_coach1")]
    public Guid? Coach1Id { get; set; }
    [Column("guid_coach2")]
    public Guid? Coach2Id { get; set; }
    [Column("guid_coach3")]
    public Guid? Coach3Id { get; set; }
    [Column("guid_coach4")]
    public Guid? Coach4Id { get; set; }
    
    public Guid? TeamPhoto { get; set; }
    public int Order { get; set; }
    public string TreneriKategorie { get; set; }
}