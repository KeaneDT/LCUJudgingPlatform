using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WEB_Assignment_Team4.Models
{
    public class JudgeAssign
    {
        
        [Display(Name = "Competition ID")]
        public int CompetitionID { get; set; }

        [Display(Name = "Judge ID")]
        public int JudgeID { get; set; }
    }
}
