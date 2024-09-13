using FluentValidation;
using TasteTrailUserExperience.Core.Feedbacks.Dtos;

namespace TasteTrailUserExperience.Api.Feedbacks.Validators;

public class FeedbackCreateDtoValidator : AbstractValidator<FeedbackCreateDto>
{
    public FeedbackCreateDtoValidator()
    {
        RuleFor(f => f.VenueId)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(f => f.Rating)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(5);
    }
}