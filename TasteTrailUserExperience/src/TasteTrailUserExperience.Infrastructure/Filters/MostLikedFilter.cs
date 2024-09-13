using TasteTrailData.Core.Filters.Specifications;
using TasteTrailUserExperience.Core.Common.Models;

namespace TasteTrailUserExperience.Infrastructure.Filters;

public class MostLikedFilter<T> : IFilterSpecification<T> where T : ILikeable
{
    public IQueryable<T> Apply(IQueryable<T> query)
    {
        return query.OrderByDescending(e => e.Likes);
    }
}
