using System.Collections.Generic;
using System.Text;
using Conference.Core.Entities.Questions.Options;

namespace Conference.Core.Entities.Questions
{
    public class SingleOptionQuestion : Question
    {
        public virtual ICollection<QuestionOption> Options { get; set; }

        public override string ToJson()
        {
            var builder = new StringBuilder();
            foreach (var option in this.Options)
            {
                builder.Append(option.ToJson());
                builder.Append(",");
            }

            builder.Remove(builder.Length - 1, 1);

            return $"{{ {this.GetFieldsForJson()}, \"Type\" : \"{nameof(SingleOptionQuestion)}\", \"{nameof(this.Options)}\" : [{builder.ToString()}] }}";
        }
    }
}