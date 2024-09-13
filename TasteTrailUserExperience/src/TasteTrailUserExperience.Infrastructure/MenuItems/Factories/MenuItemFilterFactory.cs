using TasteTrailData.Core.Filters.Enums;
using TasteTrailData.Core.Filters.Specifications;
using TasteTrailUserExperience.Core.MenuItems.Models;
using TasteTrailUserExperience.Infrastructure.Filters;

namespace TasteTrailUserExperience.Infrastructure.MenuItems.Factories;

public class MenuItemFilterFactory
{
    public static IFilterSpecification<MenuItem>? CreateFilter(FilterType? filterType)
    {
        if (filterType is null)
            return null;

        return filterType switch
        {
            FilterType.MostLiked => new MostLikedFilter<MenuItem>(),
            _ => throw new ArgumentException("Invalid filter type", filterType.ToString())
        };
    }
}
