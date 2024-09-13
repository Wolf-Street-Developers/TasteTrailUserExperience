using TasteTrailData.Core.Filters.Specifications;
using TasteTrailUserExperience.Core.Common.Models;

namespace TasteTrailUserExperience.Infrastructure.Filters;

public class HighestRatedFilter<T> : IFilterSpecification<T> where T : IRateable
{
    public IQueryable<T> Apply(IQueryable<T> query)
    {
        return query.OrderByDescending(e => e.Rating);
    }
}
