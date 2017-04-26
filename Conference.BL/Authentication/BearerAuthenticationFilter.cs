using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Filters;
using Conference.DAL;
using Conference.Core.Entities;

namespace Conference.BL.Authentication
{
    public class BearerAuthenticationFilter : IAuthenticationFilter
    {
        private ConferenceContext Context => GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(ConferenceContext)) as ConferenceContext;

        public bool AllowMultiple => false;

        public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                Respondent respondent = null;

                AuthenticationHeaderValue header = context.Request.Headers.Authorization;
                if (header != null && header.Scheme == UserIndentity.AuthenticationTypeName)
                {
                    Guid key;
                    if (Guid.TryParse(header.Parameter, out key))
                    {
                        respondent = this.Context.Respondents.FirstOrDefault(x => x.SessionId == key);
                    }
                }

                if (respondent == null)
                {
                    respondent = new Respondent { SessionId = Guid.NewGuid() };

                    this.Context.Respondents.Add(respondent);
                    this.Context.SaveChanges();
                }

                context.Principal = new PrincipalProvider(respondent.SessionId.ToString());
            }, cancellationToken);
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return Task.Run(() => { }, cancellationToken);
        }
    }
}