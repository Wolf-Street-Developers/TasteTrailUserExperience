using TasteTrailData.Core.Filters.Enums;
using TasteTrailData.Core.Filters.Specifications;
using TasteTrailUserExperience.Core.Feedbacks.Models;
using TasteTrailUserExperience.Infrastructure.Filters;

namespace TasteTrailUserExperience.Infrastructure.Feedbacks.Factories;

public class FeedbackFilterFactory
{
    public static IFilterSpecification<Feedback>? CreateFilter(FilterType? filterType)
    {
        if (filterType is null)
            return null;

        return filterType switch
        {
            FilterType.MostLiked => new MostLikedFilter<Feedback>(),
            FilterType.NewestFirst => new NewestFirstFilter<Feedback>(),
            FilterType.OldestFirst => new OldestFirstFilter<Feedback>(),
            FilterType.HighestRated => new HighestRatedFilter<Feedback>(),
            FilterType.LowestRated => new LowestRatedFilter<Feedback>(),
            _ => throw new ArgumentException("Invalid filter type", filterType.ToString())
        };
    }
}
