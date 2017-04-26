using System.Security.Principal;

namespace Conference.BL.Authentication
{
    internal class UserIndentity : IIdentity
    {
        public static string AuthenticationTypeName => "Bearer";

        public UserIndentity(string name)
        {
            this.Name = name;
        }

        public string Name { get; private set; }

        public string AuthenticationType => AuthenticationTypeName;

        public bool IsAuthenticated => this.Name == null;
    }
}