using System.Collections.Generic;

namespace Conference.Core.Entities.Answers
{
    public class RatingAnswer : Answer
    {
        public virtual ICollection<RatingOptionAnswer> RatingOptionAnswers { get; set; }
    }
}