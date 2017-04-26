using Conference.Core.Entities.Questions.Options;

namespace Conference.Core.Entities.Answers
{
    public class SingleOptionAnswer : Answer
    {
        public virtual QuestionOption Option { get; set; }
    }
}