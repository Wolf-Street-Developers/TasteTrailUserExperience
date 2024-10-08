using TasteTrailUserExperience.Core.Common.Exceptions;
using TasteTrailUserExperience.Core.FeedbackLikes.Dtos;
using TasteTrailUserExperience.Core.FeedbackLikes.Models;
using TasteTrailUserExperience.Core.FeedbackLikes.Repositories;
using TasteTrailUserExperience.Core.FeedbackLikes.Services;
using TasteTrailUserExperience.Core.Feedbacks.Models;
using TasteTrailUserExperience.Core.Feedbacks.Repositories;
using TasteTrailUserExperience.Core.Users.Models;

namespace TasteTrailUserExperience.Infrastructure.FeedbackLikes.Services;

public class FeedbackLikeService : IFeedbackLikeService
{
    private readonly IFeedbackLikeRepository _feedbackLikeRepository;
    
    private readonly IFeedbackRepository _feedbackRepository;

    public FeedbackLikeService(IFeedbackLikeRepository feedbackLikeRepository, IFeedbackRepository feedbackRepository)
    {
        _feedbackLikeRepository = feedbackLikeRepository;
        _feedbackRepository = feedbackRepository;
    }

    public async Task<int> CreateFeedbackLikeAsync(FeedbackLikeCreateDto feedbackLikeCreateDto, User user)
    {
        var feedbackLikeToCreate = new FeedbackLike()
        {
            FeedbackId = feedbackLikeCreateDto.FeedbackId,
            UserId = user.Id,
        };

        var exists = await _feedbackLikeRepository.Exists(feedbackLikeToCreate.FeedbackId, feedbackLikeToCreate.UserId);

        if (exists)
            throw new InvalidOperationException($"You already liked this feedback.");

        var id = await _feedbackLikeRepository.CreateAsync(feedbackLikeToCreate);

        var feedbackId = await _feedbackRepository.IncrementLikesAsync(new Feedback()
        {
            Id = feedbackLikeToCreate.FeedbackId,
            UserId = user.Id,
        });

        if(feedbackId is null)
        {
            await _feedbackLikeRepository.DeleteByIdAsync(id);
        }

        return id;
    }

    public async Task<int?> DeleteFeedbackLikeByIdAsync(int feedbackId, User user)
    {
        if (feedbackId <= 0)
            throw new ArgumentException($"Invalid ID value: {feedbackId}.");

        var feedbackLikeToDelete = await _feedbackLikeRepository.GetByFeedbackAndUserId(feedbackId, user.Id);

        if (feedbackLikeToDelete is null)
            return null;

        var feedbackLikeId = await _feedbackLikeRepository.DeleteByIdAsync(feedbackLikeToDelete.Id);
        
        var feedbackToDecrementId = await _feedbackRepository.DecrementLikesAsync(new Feedback()
        {
            Id = feedbackLikeToDelete.FeedbackId,
            UserId = user.Id,
        });

        if(feedbackToDecrementId is null)
        {
            await _feedbackLikeRepository.CreateAsync(new FeedbackLike()
            {
                FeedbackId = feedbackLikeToDelete.FeedbackId,
                UserId = user.Id,
            });
        }

        return feedbackLikeId;
    }
}
