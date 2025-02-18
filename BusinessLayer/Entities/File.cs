using RepoDb.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Entities
{
    [Map("sch_miners.tbl_file")]
    public class File : GuidEntityBase
    {
        [Map("filename")]
        public string Filename { get; set; }
        [Map("size")]
        public int Size { get; set; }
    }
}
