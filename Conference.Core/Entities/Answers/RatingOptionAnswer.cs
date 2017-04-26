using Conference.Core.Entities.Questions;

namespace Conference.Core.Entities.Answers
{
    public class RatingOptionAnswer: BaseEntity
    {
        public int Rating { get; set; }

        public virtual RatingOptionQuestion RatingOptionQuestion { get; set; }
    }
}