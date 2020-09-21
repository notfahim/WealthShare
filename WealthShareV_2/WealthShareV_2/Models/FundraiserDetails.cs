using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WealthShareV_2.Models
{
    public class FundraiserDetails
    {





        public int Id_issue { get; set; }
        public int UserId { get; set; }
       
        public string Title { get; set; }
        public string Issue_Description { get; set; }
        public decimal Goal { get; set; }
        public Nullable<decimal> CurrentAmount { get; set; }
        public string IssueStatus { get; set; }
        public string issueImgPath { get; set; }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public string Occupation { get; set; }
        public string AccountStatus { get; set; }
        public string imgPath { get; set; }


    }
}