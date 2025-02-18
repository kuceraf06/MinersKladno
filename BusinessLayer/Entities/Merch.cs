using RepoDb.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Entities
{
    [Map("sch_miners.tbl_merch")]
    public class Merch : GuidEntityBase
    {

        [Map("title")]
        public string Title { get; set; }
        [Map("price")]
        public int Price { get; set; }
        [Map("categories")]
        public string Categories { get; set; }
        [Map("sizes")]
        public string Sizes { get; set; }
        [Map("notice")]
        public string Notice { get; set; }
        [Map("type")]
        public string Type { get; set; }
    }
}
