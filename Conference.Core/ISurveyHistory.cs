using System.Collections.Generic;
using Conference.Core.Entities.Answers;

namespace Conference.Core
{
    public interface ISurveyHistory
    {
        IReadOnlyCollection<Answer> Answers { get; }

        void AnswerQuestion(Answer answer);
    }    
}