using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WEB_Assignment_Team4.Models
{
    public class JudgeAssign
    {
        [Display(Name = "Judge ID")]
        public int JudgeID { get; set; }
        
        [Display(Name = "Competition ID")]
        public int CompetitionID { get; set; }
    }
}
