using RepoDb.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Entities
{
    [Map("sch_miners.cis_merch_type")]
    public class CisType
    {
        [Map("code")]
        public string Code { get; set; }
        [Map("title")]
        public string Title { get; set; }
    }
}
