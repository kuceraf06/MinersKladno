using RepoDb.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Entities
{
    public abstract class GuidEntityBase
    {
        [Map("guid")]
        [Primary]
        public Guid Guid { get; set; }
    }
}
