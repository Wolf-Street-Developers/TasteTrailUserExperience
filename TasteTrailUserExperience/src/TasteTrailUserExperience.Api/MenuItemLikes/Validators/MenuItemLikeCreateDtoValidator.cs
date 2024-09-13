using FluentValidation;
using TasteTrailUserExperience.Core.MenuItemLikes.Dtos;

namespace TasteTrailUserExperience.Api.MenuItemLikes.Validators;

public class MenuItemLikeCreateDtoValidator : AbstractValidator<MenuItemLikeCreateDto>
{
    public MenuItemLikeCreateDtoValidator()
    {
        RuleFor(f => f.MenuItemId)
            .NotEmpty()
            .GreaterThan(0);
    }
}
