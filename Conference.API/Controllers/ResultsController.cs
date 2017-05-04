using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Conference.API.Models;
using Conference.Core;
using Conference.DAL;
using OfficeOpenXml;
using OfficeOpenXml.Table;

namespace Conference.API.Controllers
{
    public class ResultsController : BaseController
    {
        private Dictionary<string, string> questions = new Dictionary<string, string>()
                {
                    { "Uid", "Uid" },
                    { "Q23", "Имя" },
                    { "Q1", "Давай познакомимся" },
                    { "Q2", "Виделись мы раньше?" },
                    { "Q3", "Что привело тебя?" },
                    { "Q4", "Встреча" },
                    { "Q6", "Регистрация" },
                    { "Q8", "Кофе-брейк?" },
                    { "Q10", "JavaScript Services" },
                    { "Q18", "SQL\'фобия" },
                    { "Q14", "Xamarin" },
                    { "Q24", "Мне понравилось" },
                    { "Q25", "Я хочу предложить" },
                    { "Q22", "Мы ещё встретимся?" }
                };

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

        [Route("results/export")]
        [HttpGet]
        public HttpResponseMessage ExportAnswers()
        {
            var response = this.Request.CreateResponse();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Results");

                var format = new ExcelTextFormat();
                format.Delimiter = '|';
                format.TextQualifier = '"';
                format.DataTypes = new[] { eDataTypes.String };

                var columnCount = this.questions.Values.Count;
                var rowNumber = 1;
                worksheet.Cells[rowNumber, 1, rowNumber, columnCount].LoadFromText(string.Join("|", this.questions.Values.ToArray()), format);
                rowNumber++;

                var answers = this.Context.Database.SqlQuery<ResultAnswer>("EXEC [dbo].[GetResults]").ToArray();
                var allRespondents = answers.Select(x => x.RespondentId).Distinct().ToArray();
                foreach (int respondentId in allRespondents)
                {
                    var row = this.GetRespondentAnswers(this.questions, answers, respondentId);
                    worksheet.Cells[rowNumber, 1, rowNumber, columnCount].LoadFromText(string.Join("|", row.Values.ToArray()), format);
                    rowNumber++;
                }

                this.FormatWorksheet(worksheet);
                
                var bytes = package.GetAsByteArray();
                response.Content = new ByteArrayContent(bytes);

                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                response.Content.Headers.Add("content-disposition", "attachment;  filename=sync.net.results.xlsx");
            }

            return response;
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

        private void FormatWorksheet(ExcelWorksheet worksheet)
        {
            worksheet.Cells.Style.WrapText = true;
            for (int i = 2; i <= worksheet.Dimension.Rows; i++)
            {
                worksheet.Row(i).Height = 40;
            }

            for (int i = 2; i <= worksheet.Dimension.Columns; i++)
            {
                worksheet.Column(i).Width = 30;
            }
        }

        private Dictionary<string, string> GetRespondentAnswers(Dictionary<string, string> questions, ResultAnswer[] answers, int respondentId)
        {
            var row = new Dictionary<string, string>();
            var respondentAnswers = answers.Where(x => x.RespondentId == respondentId).ToArray();
            row.Add("Uid", respondentAnswers.First().Uid);

            foreach (var pair in questions.Except(questions.Where(h => h.Key == "Uid")))
            {
                var oneQuestionAnswers = respondentAnswers.Where(q => q.QuestionId == pair.Key);
                if (oneQuestionAnswers.Count() > 0)
                {
                    var value = string.Empty;
                    foreach (var answer in oneQuestionAnswers)
                    {
                        value += $"{answer.Name} {answer.YourOption} {answer.Text} {answer.OptionText} "
                                     + $"{answer.Description} {answer.Rating}\n";
                    }

                    value = value.Replace("  ", string.Empty).Trim()
                                 .Replace("{ConferenceName}", this.Settings.ConferenceName)
                                 .Replace("{NextNumber}", this.Settings.NextNumber)
                                 .Replace("{CurrentNumber}", this.Settings.CurrentNumber);
                    row.Add(oneQuestionAnswers.First().QuestionId, value);
                }
                else
                {
                    row.Add(pair.Key, string.Empty);
                }
            }

            return row;
        }
    }
}