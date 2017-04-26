using Conference.Core.Entities.Questions;

namespace Conference.Core.Entities.Answers
{
    public abstract class Answer : BaseEntity
    {
        public virtual Respondent Respondent { get; set; }

        public virtual Question Question { get; set; }
    }
}