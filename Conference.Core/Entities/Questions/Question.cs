namespace Conference.Core.Entities.Questions
{
    public abstract class Question : BaseEntity, IJsonConvertable
    {
        public string Text { get; set; }

        public int? Order { get; set; }

        public abstract string ToJson();

        protected string GetFieldsForJson()
        {
            return $" \"{nameof(this.Id)}\" : \"{this.Id}\", \"{nameof(this.Text)}\" : \"{this.Text}\"";
        }
    }
}