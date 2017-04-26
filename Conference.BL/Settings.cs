using Conference.Core;
using System.Configuration;

namespace Conference.BL
{
    public class Settings : ISettings
    {
        public string ConferenceName { get { return ConfigurationManager.AppSettings[nameof(ConferenceName)]; } }

        public string CurrentNumber { get { return ConfigurationManager.AppSettings[nameof(CurrentNumber)]; } }

        public string NextNumber { get { return ConfigurationManager.AppSettings[nameof(NextNumber)]; } }

        public string Password { get { return ConfigurationManager.AppSettings[nameof(Password)]; } }
    }
}