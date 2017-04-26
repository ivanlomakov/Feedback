namespace Conference.Core.Entities.Questions.Options
{
    public class RedirectionQuestionOption : QuestionOption
    {
        public virtual Question Redirect { get; set; }

        public override string ToJson()
        {
            return $"{{ {this.GetJson(nameof(RedirectionQuestionOption))} }}";
        }
    }
}