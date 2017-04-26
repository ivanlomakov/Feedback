namespace Conference.Core.Entities.Questions.Options
{
    public class QuestionOption : BaseEntity, IJsonConvertable
    {
        public string Text { get; set; }

        protected string GetJson(string type)
        {
            return $"\"{nameof(this.Id)}\" : \"{this.Id}\", \"{nameof(this.Text)}\" : \"{this.Text}\", \"Type\" : \"{type}\"";
        }

        public virtual string ToJson()
        {
            return $"{{ {this.GetJson(nameof(QuestionOption))} }}";
        }
    }
}