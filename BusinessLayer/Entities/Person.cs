using RepoDb.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Entities
{
    [Map("sch_miners.tbl_person")]
    public class Person
    {
        [Map("guid")]
        [Primary]
        public Guid Guid { get; set; }
        [Map("jmeno")]
        public string Jmeno { get; set; }
        [Map("prijmeni")]
        public string Prijmeni { get; set; }
        [Map("email")]
        public string Email { get; set; }
        [Map("telefon")]
        public string Telefon { get; set; }
        [Map("pozice_tym")]
        public string PoziceTym { get; set; }
        [Map("licence")]
        public string Licence { get; set; }
        [Map("poznamka")]
        public string Poznamka { get; set; }

        public string PopisekTym
        {
            get
            {
                var ret = !string.IsNullOrEmpty(this.Poznamka) ? (this.Poznamka + ", ") : "";
                if (!string.IsNullOrEmpty(this.Licence))
                {
                    ret += this.Licence;
                }
                return ret.TrimEnd(' ').TrimEnd(',');
            }
        }
    }
}
