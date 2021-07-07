using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEB_Assignment_Team4.Models
{
    public class InterestViewModel
    {
        public List<Interest> interestList { get; set; }
        public List<Competition> competitionList { get; set; }

        public InterestViewModel()
        {
            interestList = new List<Interest>();
            competitionList = new List<Competition>();
        }
    }
}
