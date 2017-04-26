using System.Collections.Generic;
using System.Linq;
using Conference.Core;
using Conference.Core.Entities.Answers;
using Conference.DAL;

namespace Conference.BL
{
    public class SurveyHistory : ISurveyHistory
    {
        private readonly ConferenceContext context;
        private readonly IRespondent respondent;

        public SurveyHistory(ConferenceContext context, IRespondent respondent)
        {
            this.context = context;
            this.respondent = respondent;
        }

        public IReadOnlyCollection<Answer> Answers
        {
            get
            {
                return this.context.Answers.Where(x => x.Respondent.SessionId == respondent.SessionId).ToList();
            }
        }

        public void AnswerQuestion(Answer answer)
        {
            answer.Respondent = this.context.Respondents.First(x => x.SessionId == this.respondent.SessionId);

            this.context.Answers.Add(answer);
            this.context.SaveChanges();
        }
    }
}