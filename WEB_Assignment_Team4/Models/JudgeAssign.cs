using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WEB_Assignment_Team4.Models
{
    public class JudgeAssign
    {
        
        [Display(Name = "Competition ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int CompetitionID { get; set; }

        [Display(Name = "Judge ID")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int JudgeID { get; set; }
    }
}
