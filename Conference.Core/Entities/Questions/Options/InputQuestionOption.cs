namespace Conference.Core.Entities.Questions.Options
{
    public class InputQuestionOption : QuestionOption
    {
        public string InputText { get; set; }

        public override string ToJson()
        {
            return $"{{ {this.GetJson(nameof(InputQuestionOption))} }}";
        }
    }
}