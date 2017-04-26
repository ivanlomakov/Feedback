using System.Collections.Generic;
using System.Text;

namespace Conference.Core.Entities.Questions
{
    public class RatingQuestion : Question
    {
        public virtual ICollection<RatingOptionQuestion> Options { get; set; }

        public override string ToJson()
        {
            var builder = new StringBuilder();
            foreach (var option in this.Options)
            {
                builder.Append($"{option.ToJson()},");
            }
            builder.Remove(builder.Length - 1, 1);

            return $"{{{this.GetFieldsForJson()}, \"Type\" : \"{nameof(RatingQuestion)}\", \"{nameof(this.Options)}\" : [{builder.ToString()}] }}";
        }
    }
}