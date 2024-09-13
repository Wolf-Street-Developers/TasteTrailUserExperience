using TasteTrailData.Core.Filters.Specifications;
using TasteTrailData.Core.Roles.Enums;
using TasteTrailData.Infrastructure.Filters.Dtos;
using TasteTrailUserExperience.Core.Common.Exceptions;
using TasteTrailUserExperience.Core.FeedbackLikes.Repositories;
using TasteTrailUserExperience.Core.Feedbacks.Dtos;
using TasteTrailUserExperience.Core.Feedbacks.Models;
using TasteTrailUserExperience.Core.Feedbacks.Repositories;
using TasteTrailUserExperience.Core.Feedbacks.Services;
using TasteTrailUserExperience.Core.Users.Models;
using TasteTrailUserExperience.Core.Users.Repositories;
using TasteTrailUserExperience.Core.Venues.Repositories;
using TasteTrailUserExperience.Infrastructure.Feedbacks.Factories;

namespace TasteTrailUserExperience.Infrastructure.Feedbacks.Services;

public class FeedbackService : IFeedbackService
{
    private readonly IFeedbackRepository _feedbackRepository;

    private readonly IVenueRepository _venueRepository;

    private readonly IFeedbackLikeRepository _feedbackLikeRepository;

    private readonly IUserRepository _userRepository;


    public FeedbackService(IFeedbackRepository feedbackRepository,  IVenueRepository venueRepository, IFeedbackLikeRepository feedbackLikeRepository, IUserRepository userRepository)
    {
        _feedbackRepository = feedbackRepository;
        _venueRepository = venueRepository;
        _feedbackLikeRepository = feedbackLikeRepository;
        _userRepository = userRepository;
    }

    public async Task<FilterResponseDto<FeedbackGetDto>> GetFeedbacksFilteredAsync(FilterParametersDto filterParameters, int venueId, User? authenticatedUser)
    {
        if (venueId <= 0)
            throw new ArgumentException($"Invalid Venue ID: {venueId}.");

        var newFilterParameters = new FilterParameters<Feedback>() {
            PageNumber = filterParameters.PageNumber,
            PageSize = filterParameters.PageSize,
            Specification = FeedbackFilterFactory.CreateFilter(filterParameters.Type),
            SearchTerm = null
        };

        var feedbacks = await _feedbackRepository.GetFilteredByIdAsync(newFilterParameters, venueId);
        var feedbackDtos = new List<FeedbackGetDto>();

        List<int>? likedFeedbackIds = null;

        if (authenticatedUser is not null)
            likedFeedbackIds = await _feedbackLikeRepository.GetLikedFeedbacksIds(authenticatedUser.Id);

        foreach (var feedback in feedbacks)
        {
            var user = await _userRepository.GetByIdAsync(feedback.UserId);
            var isLiked = likedFeedbackIds is not null && likedFeedbackIds.Any(id => id == feedback.Id);

            ArgumentNullException.ThrowIfNull(user);

            var feedbackDto = new FeedbackGetDto
            {
                Id = feedback.Id,
                Text = feedback.Text,
                Rating = (int)feedback.Rating,
                CreationDate = feedback.CreationDate,
                Username = user.Username,
                UserId = user.Id,
                VenueId = feedback.VenueId,
                Likes = feedback.Likes,
                IsLiked = isLiked
            };

            feedbackDtos.Add(feedbackDto);
        }

        var totalFeedbacks = await _feedbackRepository.GetCountFilteredIdAsync(newFilterParameters, venueId);
        var totalPages = (int)Math.Ceiling(totalFeedbacks / (double)filterParameters.PageSize);


        var filterReponse = new FilterResponseDto<FeedbackGetDto>() {
            CurrentPage = filterParameters.PageNumber,
            AmountOfPages = totalPages,
            AmountOfEntities = totalFeedbacks,
            Entities = feedbackDtos
        };

        return filterReponse;
    }

    public async Task<FilterResponseDto<FeedbackGetDto>> GetFeedbacksFilteredAsync(FilterParametersSearchDto filterParameters, User? authenticatedUser)
    {
        var newFilterParameters = new FilterParameters<Feedback>() {
            PageNumber = filterParameters.PageNumber,
            PageSize = filterParameters.PageSize,
            Specification = FeedbackFilterFactory.CreateFilter(filterParameters.Type),
            SearchTerm = filterParameters.SearchTerm
        };

        var feedbacks = await _feedbackRepository.GetFilteredAsync(newFilterParameters);
        var feedbackDtos = new List<FeedbackGetDto>();

        List<int>? likedFeedbackIds = null;

        if (authenticatedUser is not null)
            likedFeedbackIds = await _feedbackLikeRepository.GetLikedFeedbacksIds(authenticatedUser.Id);

        foreach (var feedback in feedbacks)
        {
            var user = await _userRepository.GetByIdAsync(feedback.UserId);
            var isLiked = likedFeedbackIds is not null && likedFeedbackIds.Any(id => id == feedback.Id);

            ArgumentNullException.ThrowIfNull(user);

            var feedbackDto = new FeedbackGetDto
            {
                Id = feedback.Id,
                Text = feedback.Text,
                Rating = (int)feedback.Rating,
                CreationDate = feedback.CreationDate,
                Username = user.Username,
                UserId = user.Id,
                VenueId = feedback.VenueId,
                Likes = feedback.Likes,
                IsLiked = isLiked
            };

            feedbackDtos.Add(feedbackDto);
        }

        var totalFeedbacks = await _feedbackRepository.GetCountFilteredAsync(newFilterParameters);
        var totalPages = (int)Math.Ceiling(totalFeedbacks / (double)filterParameters.PageSize);


        var filterReponse = new FilterResponseDto<FeedbackGetDto>() {
            CurrentPage = filterParameters.PageNumber,
            AmountOfPages = totalPages,
            AmountOfEntities = totalFeedbacks,
            Entities = feedbackDtos
        };

        return filterReponse;
    }

    public async Task<FeedbackGetDto?> GetFeedbackByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException($"Invalid ID value: {id}.");

        var feedback = await _feedbackRepository.GetByIdAsync(id);
        
        if (feedback is null)
            return null;

        var user = await _userRepository.GetByIdAsync(feedback.UserId);
        ArgumentNullException.ThrowIfNull(user);

        var feedbackDto = new FeedbackGetDto
        {
            Id = feedback.Id,
            Text = feedback.Text,
            Rating = (int)feedback.Rating,
            CreationDate = feedback.CreationDate,
            Username = user.Username,
            UserId = user.Id,
            VenueId = feedback.VenueId,
        };

        return feedbackDto;
    }

    public async Task<int> GetFeedbacksCountAsync()
    {
        return await _feedbackRepository.GetCountFilteredAsync(null);
    }

    public async Task<int> CreateFeedbackAsync(FeedbackCreateDto feedback, User user)
    {
        var venue = await _venueRepository.GetByIdAsync(feedback.VenueId) ?? 
            throw new ArgumentException($"Venue by ID: {feedback.VenueId} not found.");

        var newFeedback = new Feedback
        {
            Text = feedback.Text,
            Rating = feedback.Rating,
            CreationDate = DateTime.Now.ToUniversalTime(),
            UserId = user.Id,
            VenueId = venue.Id,
        };

        var feedbackId = await _feedbackRepository.CreateAsync(newFeedback);
        await UpdateVenueRatingAsync(newFeedback.VenueId);
        
        return feedbackId;
    }

    public async Task<int?> DeleteFeedbackByIdAsync(int id, User user)
    {
        if (id <= 0)
            throw new ArgumentException($"Invalid ID value: {id}.");

        var feedback = await _feedbackRepository.GetAsNoTrackingAsync(id);

        if (feedback is null)
            return null;

        var isAdmin = user.Role == UserRoles.Admin.ToString();

        if (!isAdmin && feedback.UserId != user.Id)
            throw new ForbiddenAccessException();

        var feedbackId = await _feedbackRepository.DeleteByIdAsync(id);

        return feedbackId;
    }

    public async Task<int?> PutFeedbackAsync(FeedbackUpdateDto feedback, User user)
    {
        var feedbackToUpdate = await _feedbackRepository.GetAsNoTrackingAsync(feedback.Id);

        if (feedbackToUpdate is null)
            return null;

        if (feedbackToUpdate.UserId != user.Id)
            throw new ForbiddenAccessException();

        var venue = await _venueRepository.GetByIdAsync(feedbackToUpdate.VenueId);

        var updatedFeedback = new Feedback
        {
            Id = feedback.Id,
            Text = feedback.Text,
            Rating = feedback.Rating,
            CreationDate = DateTime.Now.ToUniversalTime(),
            UserId = user.Id
        };

        var feedbackId = await _feedbackRepository.PutAsync(updatedFeedback);
        await UpdateVenueRatingAsync(venue!.Id);

        return feedbackId;
    }

    private async Task UpdateVenueRatingAsync(int venueId) {
        if (venueId <= 0)
            throw new ArgumentException($"Rating Update Error. Invalid ID value: {venueId}.");

        var venue = await _venueRepository.GetByIdAsync(venueId);

        if (venue is null)
            throw new ArgumentNullException(nameof(venueId));

        var averageRating = await _feedbackRepository.GetAverageRatingAsync(venue.Id);
        venue.Rating = Math.Round(averageRating, 2);

        await _venueRepository.PutAsync(venue);
    }
}
