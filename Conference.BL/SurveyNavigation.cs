using System.Linq;
using Conference.Core;
using Conference.Core.Entities.Questions;
using Conference.DAL;

namespace Conference.BL
{
    public class SurveyNavigation : ISurveyNavigation
    {
        private readonly ConferenceContext context;
        private readonly ISurveyHistory surveyHistory;
        private readonly IRespondent respondent;

        public SurveyNavigation(ConferenceContext context, ISurveyHistory surveyHistory, IRespondent respondent)
        {
            this.context = context;
            this.surveyHistory = surveyHistory;
            this.respondent = respondent;
        }

        public Question Current
        {
            get
            {
                Question current;

                var currentRespondent = this.context.Respondents.First(x => x.SessionId == this.respondent.SessionId);

                var questions = this.surveyHistory.Answers.Select(x => x.Question).ToArray();
                if (questions.Length > 0)
                {
                    current = currentRespondent.CurrentQuestion;
                }
                else
                {
                    current = this.context.Questions.FirstOrDefault(x => x.Order == 1);
                    currentRespondent.CurrentQuestion = current;
                    context.SaveChanges();
                }

                return current;
            }
        }

        public Question Next
        {
            get
            {
                int nextOrder = this.GetCurrentOrder() + 1;
                var next = this.context.Questions.FirstOrDefault(x => x.Order == nextOrder);

                return next;
            }
        }

        private int GetCurrentOrder()
        {
            int questionsOrder = 0;
            int answeredOrder = 0;

            var questions = this.surveyHistory.Answers.Select(x => x.Question).ToArray();
            if(questions.Length > 0)
            {
                answeredOrder = questions.Where(x => x.Order.HasValue).Max(x => x.Order.Value);
            }
            
            if (this.Current.Order.HasValue)
            {
                questionsOrder = this.Current.Order.Value;
            }

            return answeredOrder > questionsOrder ? answeredOrder : questionsOrder;
        }
    }
}