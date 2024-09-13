using TasteTrailData.Core.Filters.Specifications;
using TasteTrailUserExperience.Core.Common.Models;

namespace TasteTrailUserExperience.Infrastructure.Filters;

public class MostFeedbacksFilter<T> : IFilterSpecification<T> where T : IFeedbackable
{
    public IQueryable<T> Apply(IQueryable<T> query)
    {
        return query.OrderByDescending(e => e.Feedbacks.Count);
    }
}
