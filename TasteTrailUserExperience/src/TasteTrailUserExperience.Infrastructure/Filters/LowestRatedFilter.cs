using TasteTrailData.Core.Filters.Specifications;
using TasteTrailUserExperience.Core.Common.Models;

namespace TasteTrailUserExperience.Infrastructure.Filters;

public class LowestRatedFilter<T> : IFilterSpecification<T> where T : IRateable
{
    public IQueryable<T> Apply(IQueryable<T> query)
    {
        return query.OrderBy(e => e.Rating);
    }
}
