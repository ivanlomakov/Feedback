using Conference.Core.Entities.Answers;
using Conference.Core.Entities.Questions;
using System.Collections.Generic;

namespace Conference.Core
{
    public interface ISurvey
    {
        Question Current { get; }

        Question Next { get; }

        void AnswerQuestion(Answer answer);

        IReadOnlyCollection<Answer> Answers { get; }
    }
}