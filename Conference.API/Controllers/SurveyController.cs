using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;
using Conference.API.Models;
using Conference.Core;
using Conference.Core.Entities;
using Conference.Core.Entities.Answers;
using Conference.Core.Entities.Questions;
using Conference.Core.Entities.Questions.Options;
using Conference.DAL;

namespace Conference.API.Controllers
{
    public class SurveyController : BaseController
    {
        private readonly ISurvey survey;

        private readonly Dictionary<string, Func<SingleOptionQuestionModel, HttpResponseMessage>> optionAnswers;

        public SurveyController(ISettings settings, ConferenceContext context, ISurvey survey) : base(settings, context)
        {
            this.survey = survey;

            this.optionAnswers = new Dictionary<string, Func<SingleOptionQuestionModel, HttpResponseMessage>>
            {
                { nameof(this.QuestionOption), this.QuestionOption },
                { nameof(this.InputQuestionOption), this.InputQuestionOption },
                { nameof(this.RedirectionQuestionOption), this.RedirectionQuestionOption }
            };
        }

        [Route("start")]
        [HttpGet]
        public HttpResponseMessage Start()
        {
            return this.Json(this.GetJsonQuestion(this.survey.Current));
        }

        [Route("answer/TextQuestion")]
        [HttpPost]
        public HttpResponseMessage AnswerTextQuestion(TextQuestionModel model)
        {
            var current = this.survey.Current as TextQuestion;
            var answer = new TextAnswer { Question = current, Text = model.Text };
            this.AnswerQuestion(answer, this.survey.Next);

            return this.Json(this.GetJsonQuestion(this.survey.Current));
        }

        [Route("answer/SingleOptionQuestion")]
        [HttpPost]
        public HttpResponseMessage AnswerSingleOptionQuestion(SingleOptionQuestionModel model)
        {
            return this.optionAnswers[model.OptionType](model);
        }

        [Route("answer/RatingQuestion")]
        [HttpPost]
        public HttpResponseMessage AnswerRatingQuestion(RatingQuestionModel[] model)
        {
            var current = this.survey.Current as RatingQuestion;
            var options = model.Select(x => new RatingOptionAnswer
                                            {
                                                Rating = x.Rating,
                                                RatingOptionQuestion = this.Context.RatingOptionQuestions.First(r => r.Id == x.Id)
                                            }).ToList();
            var answer = new RatingAnswer { Question = current, RatingOptionAnswers = options };
            this.AnswerQuestion(answer, this.survey.Next);

            return this.Json(this.GetJsonQuestion(this.survey.Current));
        }

        [Route("answer/AcquaintanceQuestion")]
        [HttpPost]
        public HttpResponseMessage AnswerAcquaintanceQuestion(AcquaintanceQuestionModel model)
        {
            if (!this.ModelState.IsValid)
            {
                throw this.GetValidationError();
            }

            var current = this.survey.Current as AcquaintanceQuestion;

            this.UpdateRespondent(model, current);

            var answer = new AcquaintanceAnswer { Question = current, Name = model.Name, Uid = model.Uid };
            this.AnswerQuestion(answer, this.survey.Next);

            return this.Json(this.GetJsonQuestion(this.survey.Current));
        }

        private HttpResponseMessage QuestionOption(SingleOptionQuestionModel model)
        {
            var current = this.survey.Current as SingleOptionQuestion;
            var option = current.Options.First(x => x.Id == model.OptionId);

            var answer = new SingleOptionAnswer { Question = current,  Option = option };
            this.AnswerQuestion(answer, this.survey.Next);

            return this.Json(this.GetJsonQuestion(this.survey.Current));
        }

        private HttpResponseMessage InputQuestionOption(SingleOptionQuestionModel model)
        {
            var current = this.survey.Current as SingleOptionQuestion;
            var option = current.Options.First(x => x.Id == model.OptionId) as InputQuestionOption;

            var answer = new SingleInputOptionAnswer { Question = current, Option = option, OptionText = model.Text };
            this.AnswerQuestion(answer, this.survey.Next);

            return this.Json(this.GetJsonQuestion(this.survey.Current));
        }

        private HttpResponseMessage RedirectionQuestionOption(SingleOptionQuestionModel model)
        {
            var current = this.survey.Current as SingleOptionQuestion;
            var option = current.Options.First(x => x.Id == model.OptionId) as RedirectionQuestionOption;

            var answer = new SingleOptionAnswer { Question = current, Option = option };
            this.AnswerQuestion(answer, option.Redirect);

            return this.Json(this.GetJsonQuestion(option.Redirect));
        }

        [Route("checkpassword")]
        [HttpPost]
        public HttpResponseMessage CheckPassword([FromBody]string password)
        {
            var result = HttpStatusCode.Unauthorized;

            if (password == this.Settings.Password)
            {
                result = HttpStatusCode.OK;
            }

            return this.Request.CreateResponse(result);
        }

        [Route("winner")]
        [ResponseType(typeof(WinnerModel))]
        public HttpResponseMessage GetWinner(string password)
        {
            if (password != this.Settings.Password)
            {
                return this.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }

            var result = new WinnerModel();

            var respondent = this.GetRandomRespondent();
            if (respondent != null)
            {
                result.Uid = respondent.Uid;
                result.Name = respondent.Name;
            }            

            return this.Request.CreateResponse(HttpStatusCode.OK, result);
        }

        private Respondent GetRandomRespondent()
        {
            Respondent respondent = null;

            var selectedUids = this.Context.SelectedUids.Select(x => x.Uid).ToArray();
            var allUids = this.Context.Respondents.Select(x => x.Uid).Where(x => !string.IsNullOrEmpty(x)).Distinct().ToArray().Except(selectedUids).ToArray();
            if (allUids.Length > 0)
            {
                var randomUid = allUids[this.GetRandomNumber((uint)allUids.Length)];

                this.Context.SelectedUids.Add(new SelectedUid { Uid = randomUid });
                this.Context.SaveChanges();

                respondent = this.Context.Respondents.First(x => x.Uid == randomUid);
            }

            return respondent;            
        }

        private uint GetRandomNumber(uint maxValue)
        {
            var data = new byte[4];

            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetBytes(data);
            }

            uint value = BitConverter.ToUInt32(data, 0);
            return value % maxValue;
        }

        private void UpdateRespondent(AcquaintanceQuestionModel model, Question question)
        {
            var sessionId = Guid.Parse(this.User.Identity.Name);
            var respondent = this.Context.Respondents.First(x => x.SessionId == sessionId);
            respondent.Uid = model.Uid;
            respondent.Name = model.Name;
            this.Context.SaveChanges();
        }

        private string GetJsonQuestion(Question question)
        {
            var builder = new StringBuilder();

            builder.Append("{");
            builder.AppendFormat("\"Question\":{0},", question != null ? this.ReplaceVariables(question.ToJson()) : "{}");
            builder.AppendFormat("\"Key\":\"{0}\"", this.User.Identity.Name);
            builder.AppendFormat(",\"Quantity\":{0}", this.GetQuestionsQuantity(question) - 1);
            builder.AppendFormat(",\"Answered\":{0}", this.survey.Answers.Count() - 1);
            builder.Append("}");

            return builder.ToString();
        }

        private void AnswerQuestion(Answer answer, Question next)
        {
            this.survey.AnswerQuestion(answer);

            var sessionId = Guid.Parse(this.User.Identity.Name);
            var respondent = this.Context.Respondents.First(x => x.SessionId == sessionId);
            respondent.CurrentQuestion = next;
            this.Context.SaveChanges();
        }

        private int GetQuestionsQuantity(Question question)
        {
            var questionsWithOrder = this.Context.Questions.Count(x => x.Order.HasValue);

            if (question != null && !question.Order.HasValue)
            {
                questionsWithOrder++;
            }

            var unorderedAnsweredQuestions = this.survey.Answers.Count(x => !x.Question.Order.HasValue);

            return questionsWithOrder + unorderedAnsweredQuestions;
        }

        private HttpResponseException GetValidationError()
        {
            var firstError = this.ModelState.Select(x => x.Value).Where(x => x.Errors.Any()).SelectMany(x => x.Errors).Select(x => x.ErrorMessage).FirstOrDefault();

            return new HttpResponseException(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ReasonPhrase = firstError
                });
        }
    }
}