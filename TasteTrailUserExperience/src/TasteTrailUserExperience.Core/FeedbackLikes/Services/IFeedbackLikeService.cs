using TasteTrailUserExperience.Core.FeedbackLikes.Dtos;
using TasteTrailUserExperience.Core.Users.Models;

namespace TasteTrailUserExperience.Core.FeedbackLikes.Services;

public interface IFeedbackLikeService
{
    Task<int> CreateFeedbackLikeAsync(FeedbackLikeCreateDto feedbackLikeCreateDto, User user);

    Task<int?> DeleteFeedbackLikeByIdAsync(int id, User user);
}
