using System.ComponentModel.DataAnnotations.Schema;
using BusinessLayer.Entities;
using RepoDb.Attributes;

namespace Miners.Web.BusinessLayer.Entities
{
    [Table("tbl_person", Schema = "sch_miners")]
    public class Person : GuidEntityBase
    {
        public string Jmeno { get; set; }
        public string Prijmeni { get; set; }
        public string? Email { get; set; }
        public string? Telefon { get; set; }
        public string? PoziceTym { get; set; }
        public string? Licence { get; set; }
        public string? Poznamka { get; set; }
        public byte[]? Foto { get; set; }

        // public string PopisekTym
        // {
        //     get
        //     {
        //         var ret = !string.IsNullOrEmpty(this.Poznamka) ? (this.Poznamka + ", ") : "";
        //         if (!string.IsNullOrEmpty(this.Licence))
        //         {
        //             ret += this.Licence;
        //         }
        //         return ret.TrimEnd(' ').TrimEnd(',');
        //     }
        // }
    }
}