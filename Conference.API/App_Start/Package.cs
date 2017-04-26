using Conference.BL;
using Conference.Core;
using Conference.DAL;
using SimpleInjector;
using SimpleInjector.Packaging;

namespace Conference.API
{
    public class Package : IPackage
    {
        public void RegisterServices(Container container)
        {
            container.Register<ConferenceContext>(Lifestyle.Scoped);
            container.Register<ISurveyHistory, SurveyHistory>(Lifestyle.Scoped);
            container.Register<ISurveyNavigation, SurveyNavigation>(Lifestyle.Scoped);
            container.Register<ISettings, Settings>(Lifestyle.Scoped);
            container.Register<ISurvey, Survey>(Lifestyle.Scoped);
            container.Register<IRespondent, RespondentInfo>(Lifestyle.Scoped);
        }
    }
}