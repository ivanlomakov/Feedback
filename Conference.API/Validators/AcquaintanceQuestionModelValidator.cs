using Conference.API.Models;
using FluentValidation;

namespace Conference.API.Validators
{
    public class AcquaintanceQuestionModelValidator : AbstractValidator<AcquaintanceQuestionModel>
    {
        public AcquaintanceQuestionModelValidator()
        {
            this.CascadeMode = CascadeMode.StopOnFirstFailure;

            this.RuleFor(x => x.Name).NotEmpty().WithMessage("Name must not be empty")
                                        .Matches(@"^[a-zA-Zа-яА-Я ]+$").WithMessage("Only letters are allowed");

            this.RuleFor(x => x.Uid).NotEmpty().WithMessage("Uid must not be empty")
                                        .Matches(@"^[0-6]\d{2}$").WithMessage("Only three numbers are allowed")
                                        .Must(x => x.Length == 3 && int.Parse(x) > 0 && int.Parse(x) <= 600).WithMessage("Invalid uid number");
        }
    }
}