using System.Security.Principal;

namespace Conference.BL.Authentication
{
    public class PrincipalProvider : IPrincipal
    {
        public PrincipalProvider(string name)
        {
            this.Identity = new UserIndentity(name);
        }

        public bool IsInRole(string role)
        {
            return true;
        }

        public IIdentity Identity { get; private set; }
    }
}