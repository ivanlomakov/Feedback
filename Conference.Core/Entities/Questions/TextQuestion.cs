namespace Conference.Core.Entities.Questions
{
    public class TextQuestion : Question
    {
        public override string ToJson()
        {
            return $"{{ {this.GetFieldsForJson()}, \"Type\" : \"{nameof(TextQuestion)}\" }}";
        }
    }
}