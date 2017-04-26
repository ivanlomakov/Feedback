using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Conference.API.Models;
using Conference.Core;
using Conference.DAL;

namespace Conference.API.Controllers
{
    public class ResultsController : BaseController
    {
        public ResultsController(ISettings settings, ConferenceContext context) : base(settings, context)
        {
        }

        [Route("results")]
        [HttpPost]
        public HttpResponseMessage Answers([FromBody]string password)
        {
            if (password != this.Settings.Password)
            {
                return this.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }

            var answers = this.Context.Database.SqlQuery<ResultAnswer>("EXEC [dbo].[GetResults]").ToArray();

            var result = this.ComposeAnswersForAllUsers(answers)
                                .Replace("\r\n", string.Empty)
                                .Replace("<", "&lt;")
                                .Replace(">", "&gt;")
                                .Replace("\\", "&#92;");

            return this.Json(this.ReplaceVariables(result));
        }

        private string ComposeAnswersForAllUsers(ResultAnswer[] answers)
        {
            var allRespondents = answers.Select(x => x.RespondentId).Distinct().ToArray();
            var builder = new StringBuilder();
            builder.Append("[");
            foreach (int respondentId in allRespondents)
            {
                var respondentAnswers = answers.Where(x => x.RespondentId == respondentId).ToArray();

                var answer = this.GetAnswersForUser(respondentAnswers);
                builder.Append($"{answer}, ");
            }

            if (allRespondents.Length > 0)
            {
                builder.Remove(builder.Length - 2, 1);
            }

            builder.Append("]");

            return builder.ToString();
        }

        private string GetAnswersForUser(ResultAnswer[] answers)
        {
            var options = new Dictionary<string, string>();
            foreach (ResultAnswer answer in answers)
            {
                var key = answer.QuestionId;
                var value = $"{answer.Name} {answer.YourOption} {answer.Text} {answer.OptionText} "
                                + $"{answer.Description} {answer.Rating}";
                value = value.Replace("  ", string.Empty).Replace("\"", "&quot;").Replace("'", "&#39;").Trim();

                if (!options.ContainsKey(key))
                {
                    options.Add(key, value);
                }
                else
                {
                    options[key] = $"{options[key]}, {value}";
                }
            }

            var uid = answers.First(x => !string.IsNullOrEmpty(x.Uid)).Uid.ToString();

            return this.ComposeUserAnswersInRow(options, uid);
        }

        private string ComposeUserAnswersInRow(Dictionary<string, string> options, string uid)
        {
            var builder = new StringBuilder();
            builder.Append("{");
            builder.Append($"\"Uid\" : \"{uid}\", ");
            foreach (var option in options)
            {
                builder.Append($"\"{option.Key}\" : \"{option.Value}\", ");
            }

            builder.Remove(builder.Length - 2, 1);
            builder.Append("}");

            return builder.ToString();
        }
    }
}