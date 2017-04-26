namespace Conference.Core.Entities.Questions
{
    public class RatingOptionQuestion : Question
    {
        public string Description { get; set; }

        public int From { get; set; }

        public int To { get; set; }

        public int Step { get; set; }

        public override string ToJson()
        {
            return $"{{{this.GetFieldsForJson()}, \"Type\" : \"{nameof(RatingOptionQuestion)}\", \"{nameof(this.From)}\" : {this.From}, " +
                        $"\"{nameof(this.To)}\" : {this.To}, \"{nameof(this.Step)}\" : {this.Step}, \"{nameof(this.Description)}\" : \"{this.Description}\" }}";
        }
    }
}