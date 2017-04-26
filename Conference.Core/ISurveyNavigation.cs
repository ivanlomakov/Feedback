using Conference.Core.Entities.Questions;

namespace Conference.Core
{
    public interface ISurveyNavigation
    {
        Question Current { get; }

        Question Next { get; }
    }
}