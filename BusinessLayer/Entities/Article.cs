using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessLayer.Entities;

[Table("tbl_article", Schema = "sch_miners")]
public class Article : GuidEntityBase
{
    public DateTime DateCreated { get; set; }
    public string? Title { get; set; }
    public string Text { get; set; }
    public Guid GuidImage { get; set; }
    [ForeignKey(nameof(GuidImage))]
    public File Image { get; set; } = null!;
    public bool Top { get; set; }
    public int UrlId { get; set; }
}