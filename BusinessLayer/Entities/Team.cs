using RepoDb.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BusinessLayer.Entities
{
    [Map("sch_miners.tbl_team")]
    public class Team
    {
        [Map("guid")]
        [Primary]
        public Guid Guid { get; set; }
        [Map("id")]
        public string Id { get; set; }
        [Map("title")]
        public string Title { get; set; }
        [Map("category")]
        public string Category { get; set; }
        [Map("description")]
        public string Description { get; set; }
        [Map("training_days")]
        public string TrainingDays { get; set; }
        [Map("fee")]
        public string Fee { get; set; }
        [Map("coaches")]
        public string Coaches { get; set; }
        [Map("league")]
        public string League { get; set; }
        [Map("team_photo")]
        public Guid? TeamPhoto { get; set; }
        [Map("guid_coach1")]
        public Guid? GuidCoach1 { get; set; }
        [Map("guid_coach2")]
        public Guid? GuidCoach2 { get; set; }
        [Map("guid_coach3")]
        public Guid? GuidCoach3 { get; set; }
        [Map("guid_coach4")]
        public Guid? GuidCoach4 { get; set; }
        [Map("coach1_function")]
        public string Coach1Funkce { get; set; }
        [Map("coach2_function")]
        public string Coach2Funkce { get; set; }
        [Map("coach3_function")]
        public string Coach3Funkce { get; set; }
        [Map("coach4_function")]
        public string Coach4Funkce { get; set; }
        [Map("order")]
        public int Order { get; set; }
    }
}
