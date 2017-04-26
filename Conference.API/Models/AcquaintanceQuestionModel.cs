using Conference.API.Validators;
using FluentValidation.Attributes;

namespace Conference.API.Models
{
    [Validator(typeof(AcquaintanceQuestionModelValidator))]
    public class AcquaintanceQuestionModel
    {
        public string Name { get; set; }

        public string Uid { get; set; }
    }
}