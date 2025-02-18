using System;
using System.Text.RegularExpressions;
using RepoDb.Attributes;

namespace BusinessLayer.Entities
{
    [Map("sch_miners.tbl_article")]
    public class Article
    {
        [Map("guid")]
        [Primary]
        public Guid Guid { get; set; }
        
        [Map("date_created")]
        public DateTime DateCreated { get; set; }

        [Map("title")]
        public string Title { get; set; }

        [Map("text")]
        public string Text { get; set; }

        [Map("guid_image")]
        public Guid? GuidImage { get; set; }
        [Map("top")]
        public bool Top { get; set; }

        public string TeaserPhoto
        {
            get
            {
                if (GuidImage.HasValue)
                {
                    return GuidImage.ToString();
                }
                return "article-nophoto.jpg";
            }
        }

        public string Teaser
        {
            get
            {
                var plain = Regex.Replace(this.Text, @"<img\s[^>]*>(?:\s*?</img>)?", "", RegexOptions.IgnoreCase);
                var teaser = plain.Substring(0, Math.Min(250, plain.Length));
                if (plain.Length > teaser.Length)
                {
                    teaser += "...";
                }
                return teaser;
            }
        }

    }
}