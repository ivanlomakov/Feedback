using Conference.Core;
using Conference.Core.Entities.Answers;
using Conference.Core.Entities.Questions;
using System.Collections.Generic;

namespace Conference.BL
{
    public class Survey : ISurvey
    {
        private readonly ISurveyNavigation surveyNavigation;
        private readonly ISurveyHistory surveyHistory;

        public Survey(ISurveyNavigation surveyNavigation, ISurveyHistory surveyHistory)
        {
            this.surveyNavigation = surveyNavigation;
            this.surveyHistory = surveyHistory;
        }

        public Question Current
        {
            get
            {
                return this.surveyNavigation.Current;
            }
        }

        public Question Next
        {
            get
            {
                return this.surveyNavigation.Next;
            }
        }

        public void AnswerQuestion(Answer answer)
        {
            this.surveyHistory.AnswerQuestion(answer);
        }

        public IReadOnlyCollection<Answer> Answers
        {
            get
            {
                return this.surveyHistory.Answers;
            }
        }
    }
}