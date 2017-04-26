namespace Conference.Core.Entities.Questions
{
    public class AcquaintanceQuestion : Question
    {
        public override string ToJson()
        {
            return $"{{{this.GetFieldsForJson()}, \"Type\" : \"{nameof(AcquaintanceQuestion)}\" }}";
        }
    }
}