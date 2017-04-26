using System;
using System.Web;
using Conference.Core;

namespace Conference.BL
{
    public class RespondentInfo : IRespondent
    {
        public Guid SessionId
        {
            get
            {
                return Guid.Parse(HttpContext.Current.User.Identity.Name);
            }
        }
    }
}