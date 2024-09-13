using FluentValidation;
using TasteTrailUserExperience.Core.FeedbackLikes.Dtos;

namespace TasteTrailUserExperience.Api.FeedbackLikes.Validators;

public class FeedbackLikeCreateDtoValidator : AbstractValidator<FeedbackLikeCreateDto>
{
    public FeedbackLikeCreateDtoValidator()
    {
        RuleFor(f => f.FeedbackId)
            .NotEmpty()
            .GreaterThan(0);
    }
}
