using RepoDb.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Entities
{
    [Map("sch_miners.tbl_scoreboard")]
    public class Scoreboard
    {
        [Map("active")]
        public string Active { get; set; }
        
        [Map("typ")]
        public string Typ { get; set; }
        
        [Map("live_tv")]
        public string LiveTv { get; set; }
        
        [Map("live_tv_url")]
        public string LiveTvUrl { get; set; }
        
        [Map("url")]
        public string Url { get; set; }
        
        [Map("mb_id")]
        public string MbId { get; set; }

        [Map("home_team")]
        public string HomeTeam { get; set; }
        [Map("visitor_team")]
        public string VisitorTeam { get; set; }
        [Map("home_score")]
        public int HomeScore { get; set; }
        [Map("visitor_score")]
        public int VisitorScore { get; set; }
        [Map("first_base")]
        public bool FirstBase { get; set; }
        [Map("second_base")]
        public bool SecondBase { get; set; }
        [Map("third_base")]
        public bool ThirdBase { get; set; }
        [Map("outs")]
        public int Outs { get; set; }
        [Map("inning")]
        public int Inning { get; set; }
        [Map("top_inning")]
        public bool TopInning { get; set; }
        public bool ActiveBool
        {
            get
            {
                return Active == "Y";
            }
            set
            {
                if (value)
                {
                    this.Active = "Y";
                }
                else
                {
                    this.Active = "N";
                }
            }
        }
        public bool LiveTvBool
        {
            get
            {
                return LiveTv == "Y";
            }
            set { this.LiveTv = value ? "Y" : "N"; }
        }
    }
}
