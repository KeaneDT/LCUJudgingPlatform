using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WEB_Assignment_Team4.Models
{
    public class Comment
    {
        [Display(Name = "Comment ID")]
        public int CommentID { get; set; }

        [Display(Name = "Competition ID")]
        public int CompetitionID { get; set; }

        [Display(Name = "Comment")]
        public string Description { get; set; }

        [Display(Name = "Date And Time Posted")]
        [DataType(DataType.Date)]
        public DateTime? DateTimePosted { get; set; }
    }
}
