using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamsManager.Application.Model
{
    public class ReviewedHandIn : HandIn
    {
        public ReviewedHandIn(HandIn handIn, DateTime reviewDate, int points) : base(handIn)
        {
            ReviewDate = reviewDate;
            Points = points;
            
        }
#pragma warning disable CS8618
        protected ReviewedHandIn() { }
#pragma warning restore CS8616
        public DateTime ReviewDate { get; set; }
        public int Points { get; set; }
    }
}
