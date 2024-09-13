using TasteTrailUserExperience.Core.Common.Models;

namespace TasteTrailUserExperience.Core.Common.Repositories.Base;

public interface IDecrementLikesAsync<TEntity, TReturn> where TEntity : ILikeable
{
    Task<TReturn> DecrementLikesAsync(TEntity entity);
}