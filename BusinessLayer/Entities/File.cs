using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessLayer.Entities;

[Table("tbl_file")]
public class File : GuidEntityBase
{
    public string Filename { get; set; }
}