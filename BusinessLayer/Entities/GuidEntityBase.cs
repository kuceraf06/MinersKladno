using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessLayer.Entities;

public class GuidEntityBase
{
    [Column("guid")]
    public Guid Id { get; set; }
}