using System.ComponentModel.DataAnnotations.Schema;
using BusinessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Miners.Web.BusinessLayer.Entities;

[Index(nameof(Code), IsUnique = true)]
[Table("tbl_category", Schema = "sch_miners")]
public class Category : GuidEntityBase
{
    public required string Code { get; set; }
    public required string Title { get; set; }
}