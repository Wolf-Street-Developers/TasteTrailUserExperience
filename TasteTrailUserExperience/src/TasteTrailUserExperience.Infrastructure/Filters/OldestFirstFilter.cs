using TasteTrailData.Core.Filters.Specifications;
using TasteTrailUserExperience.Core.Common.Models;

namespace TasteTrailUserExperience.Infrastructure.Filters;

public class OldestFirstFilter<T> : IFilterSpecification<T> where T : ICreateable
{
    public IQueryable<T> Apply(IQueryable<T> query)
    {
        return query.OrderBy(e => e.CreationDate);
    }
}
