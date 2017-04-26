namespace Conference.API.Models
{
    public class ResultAnswer
    {
        public int RespondentId { get; set; }

        public string QuestionId { get; set; }

        public string QuestionText { get; set; }

        public string Name { get; set; }

        public string Uid { get; set; }

        public string YourOption { get; set; }

        public string Text { get; set; }

        public string OptionText { get; set; }

        public string Description { get; set; }

        public int? Rating { get; set; }
    }
}