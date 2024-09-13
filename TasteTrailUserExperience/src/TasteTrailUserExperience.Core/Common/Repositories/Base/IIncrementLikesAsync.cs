using TasteTrailUserExperience.Core.Common.Models;

namespace TasteTrailUserExperience.Core.Common.Repositories.Base;

public interface IIncrementLikesAsync<TEntity, TReturn> where TEntity : ILikeable
{
    Task<TReturn> IncrementLikesAsync(TEntity entity);
}
