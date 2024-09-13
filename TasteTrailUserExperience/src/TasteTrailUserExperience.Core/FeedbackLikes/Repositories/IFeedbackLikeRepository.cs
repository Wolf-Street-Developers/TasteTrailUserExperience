using TasteTrailData.Core.Common.Repositories.Base;
using TasteTrailUserExperience.Core.FeedbackLikes.Models;

namespace TasteTrailUserExperience.Core.FeedbackLikes.Repositories;

public interface IFeedbackLikeRepository : ICreateAsync<FeedbackLike, int>, IDeleteByIdAsync<int, int?>, IGetAsNoTrackingAsync<FeedbackLike, int>
{
    Task<List<int>> GetLikedFeedbacksIds(string userId);

    Task<bool> Exists(int feedbackId, string userId);
}