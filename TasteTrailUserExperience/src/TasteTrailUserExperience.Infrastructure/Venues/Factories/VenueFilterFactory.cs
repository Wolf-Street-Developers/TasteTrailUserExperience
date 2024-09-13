using TasteTrailData.Core.Filters.Enums;
using TasteTrailData.Core.Filters.Specifications;
using TasteTrailUserExperience.Infrastructure.Filters;
using TasteTrailUserExperience.Core.Venues.Models;

namespace TasteTrailUserExperience.Infrastructure.Venues.Factories;

public class VenueFilterFactory
{
    public static IFilterSpecification<Venue>? CreateFilter(FilterType? filterType)
    {
        if (filterType is null)
            return null;

        return filterType switch
        {
            FilterType.MostFeedbacks => new MostFeedbacksFilter<Venue>(),
            FilterType.NewestFirst => new NewestFirstFilter<Venue>(),
            FilterType.OldestFirst => new OldestFirstFilter<Venue>(),
            FilterType.HighestRated => new HighestRatedFilter<Venue>(),
            FilterType.LowestRated => new LowestRatedFilter<Venue>(),
            _ => throw new ArgumentException("Invalid filter type", filterType.ToString())
        };
    }
}
