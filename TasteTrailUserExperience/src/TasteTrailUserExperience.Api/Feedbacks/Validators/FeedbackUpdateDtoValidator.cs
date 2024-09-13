using FluentValidation;
using TasteTrailUserExperience.Core.Feedbacks.Dtos;

namespace TasteTrailUserExperience.Api.Feedbacks.Validators;

public class FeedbackUpdateDtoValidator : AbstractValidator<FeedbackUpdateDto>
{
    public FeedbackUpdateDtoValidator()
    {
        RuleFor(f => f.Id)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(f => f.Rating)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(5);
    }
}