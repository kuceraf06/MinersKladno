using System.Net;
using System.Xml.Linq;

namespace Miners.Web.BusinessLayer;

public class GameStatus
    {
        public GameStatus() { }
        public GameStatus(XElement metadata, string status)
        {
            //this.HomeTeam = TeamName(metadata, "ht");
            //this.VisitorTeam = TeamName(metadata, "vt");

            var values = status.Split(',');
            this.HomeScore = int.Parse(values[5]);
            this.VisitorScore = int.Parse(values[6]);
            this.FirstBase = int.Parse(values[9]) > 0;
            this.SecondBase = int.Parse(values[10]) > 0;
            this.ThirdBase = int.Parse(values[11]) > 0;
            this.Outs = int.Parse(values[2]);
            this.Inning = int.Parse(values[3]);
            this.Bottom = values[4] == "1";
        }

        public GameStatus(BusinessLayer.Entities.Scoreboard sb)
        {
            this.HomeTeam = sb.HomeTeam;
            this.VisitorTeam = sb.VisitorTeam;
            this.HomeScore = sb.HomeScore;
            this.VisitorScore = sb.VisitorScore;
            //this.FirstBase = sb.FirstBase;
            //this.SecondBase = sb.SecondBase;
            //this.ThirdBase = sb.ThirdBase;
            this.Outs = sb.Outs;
            this.Inning = sb.Inning;
            this.Bottom = !sb.TopInning;
        }

        private string TeamName(XElement metadata, string key)
        {
            var htsn = metadata.Attribute(key + "sn").Value;
            if (string.IsNullOrEmpty(htsn))
            {
                htsn = metadata.Attribute(key + "n").Value;
            }
            htsn = WebUtility.UrlDecode(htsn).ToUpper();
            return htsn.Length <= 7 ? htsn : htsn.Substring(0, 7);
        }
        public string Type { get; set; }
        public string Latest { get; set; }
        public string HomeTeam { get; set; }
        public string VisitorTeam { get; set; }
        public int HomeScore { get; set; }
        public int VisitorScore { get; set; }
        public bool FirstBase { get; set; }
        public bool SecondBase { get; set; }
        public bool ThirdBase { get; set; }
        public int Outs { get; set; }
        public int Inning { get; set; }
        public int Balls { get; set; }
        public int Strikes { get; set; }
        public bool Bottom { get; set; }
        public string Pitcher { get; set; }
        public int? PitchCount { get; set; }
        public int? PitcherStrikes { get; set; }
        public string Batter { get; set; }
        public string BatterLine { get; set; }
        public string BatterAvg { get; set; }
        public string HomeTeamFull { get; set; }
        public string AwayTeamFull { get; set; }
        public string HomeLogoUrl { get; set; }
        public string AwayLogoUrl { get; set; }
        public bool BetweenInnings { get; set; }
        public int? PlateCountType { get; set; }
        public string TeamInfoError { get; set; }
        public string?[] HomeInnings { get; set; }
        public string?[] AwayInnings { get; set; }
        public int HomeHits { get; set; }
        public int HomeErrors { get; set; }
        public int HomeLob { get; set; }
        public int AwayHits { get; set; }
        public int AwayErrors { get; set; }
        public int AwayLob { get; set; }
    }