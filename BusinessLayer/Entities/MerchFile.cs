using RepoDb.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Entities
{
    [Map("sch_miners.tbl_merch_file")]
    public class MerchFile : GuidEntityBase
    {
        [Map("guid_merch")]
        public Guid GuidMerch { get; set; }
        [Map("guid_file")]
        public Guid GuidFile { get; set; }
        [Map("guid_thumb")]
        public Guid GuidThumb { get; set; }
    }
}
