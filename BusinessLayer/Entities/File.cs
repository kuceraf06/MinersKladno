using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessLayer.Entities;

[Table("tbl_file", Schema = "sch_miners")]
public class File : GuidEntityBase
{
    public string Filename { get; set; }
    public int Size { get; set; }
    public byte[]? Data { get; set; }
}