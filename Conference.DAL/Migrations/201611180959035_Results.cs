using System.Data.Entity.Migrations;
using Conference.Core.Entities;
using Conference.Core.Entities.Answers;
using Conference.Core.Entities.Questions;
using Conference.Core.Entities.Questions.Options;

namespace Conference.DAL.Migrations
{ 
    public partial class Results : DbMigration
    {
        private const string PROCEDURE_NAME = "GetResults";
        private string body = $@"
SELECT respondents.[{nameof(Respondent.Id)}] as [RespondentId],
	   'Q' + CONVERT(varchar(10), questions.[{nameof(Question.Id)}]) as [QuestionId],
	   answers.[{nameof(AcquaintanceAnswer.Name)}],
	   answers.[{nameof(AcquaintanceAnswer.Uid)}],
	   answers.[{nameof(SingleInputOptionAnswer.OptionText)}] as [YourOption], 
	   answers.[{nameof(TextAnswer.Text)}],
	   questionOptions.[Text] as [OptionText],
	   ratingOptionAnswers.[Description],
	   ratingOptionAnswers.[Rating]
	FROM [dbo].[{nameof(Answer)}s] answers
		INNER JOIN [dbo].[{nameof(Question)}s] questions ON questions.[{nameof(Question.Id)}] = answers.[{nameof(Answer.Question)}Id]
		INNER JOIN [dbo].[{nameof(Respondent)}s] respondents ON respondents.[{nameof(Respondent.Id)}] = answers.[{nameof(Answer.Respondent)}Id]
		INNER JOIN
			(
				SELECT [{nameof(QuestionOption.Id)}],
					   [{nameof(QuestionOption.Text)}],
					   [{nameof(InputQuestionOption.InputText)}]
					FROM [dbo].[{nameof(QuestionOption)}s]

					UNION

				SELECT NULL, '', ''
			) questionOptions ON questionOptions.[{nameof(QuestionOption.Id)}] = answers.[{nameof(SingleOptionAnswer.Option)}Id] 
					   			 OR (questionOptions.[{nameof(SingleOptionAnswer.Id)}] IS NULL AND answers.[{nameof(SingleOptionAnswer.Option)}Id] IS NULL)
		LEFT OUTER JOIN
			(
				SELECT ratingOptionAnswers.[{nameof(RatingOptionAnswer.Id)}],
					   ratingOptionAnswers.[{nameof(RatingOptionAnswer.Rating)}],
					   ratingOptionAnswers.[{nameof(RatingOptionAnswer.Rating)}AnswerId],
					   questions.[{nameof(RatingOptionQuestion.Description)}]
					FROM [dbo].[{nameof(RatingOptionAnswer)}s] ratingOptionAnswers
						INNER JOIN [dbo].[{nameof(Question)}s] questions ON ratingOptionAnswers.[{nameof(RatingOptionAnswer.RatingOptionQuestion)}Id] = questions.[{nameof(Question.Id)}]
			) ratingOptionAnswers ON ratingOptionAnswers.[{nameof(RatingOptionAnswer.Rating)}AnswerId] = answers.[{nameof(Answer.Id)}]
	ORDER BY answers.[{nameof(Answer.Id)}]";

        public override void Up()
        {
            this.CreateStoredProcedure(PROCEDURE_NAME, this.body);
        }
        
        public override void Down()
        {
            this.DropStoredProcedure(PROCEDURE_NAME);
        }
    }
}
