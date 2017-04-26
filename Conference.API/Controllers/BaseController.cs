using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Conference.Core;
using Conference.DAL;

namespace Conference.API.Controllers
{
    public class BaseController : ApiController
    {
        private const string CONTENT_TYPE = "application/json";

        protected ISettings Settings { get; set; }

        protected ConferenceContext Context { get; set; }

        public BaseController(ISettings settings, ConferenceContext context)
        {
            this.Settings = settings;
            this.Context = context;
        }

        protected string ReplaceVariables(string text)
        {
            return text.Replace($"{{{nameof(this.Settings.ConferenceName)}}}", this.Settings.ConferenceName)
                        .Replace($"{{{nameof(this.Settings.CurrentNumber)}}}", this.Settings.CurrentNumber)
                        .Replace($"{{{nameof(this.Settings.NextNumber)}}}", this.Settings.NextNumber);
        }

        protected HttpResponseMessage Json(string json)
        {
            var response = this.Request.CreateResponse(HttpStatusCode.OK);

            response.Content = new StringContent(json, Encoding.UTF8, CONTENT_TYPE);
            return response;
        }
    }
}