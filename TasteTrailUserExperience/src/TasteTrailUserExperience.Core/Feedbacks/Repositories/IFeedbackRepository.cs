using TasteTrailData.Core.Common.Repositories.Base;
using TasteTrailUserExperience.Core.Feedbacks.Models;

namespace TasteTrailUserExperience.Core.Feedbacks.Repositories;

public interface IFeedbackRepository : IGetFilteredByIdAsync<Feedback, int>, IGetFilteredAsync<Feedback>,
IGetAsNoTrackingAsync<Feedback?, int>, IGetCountFilteredIdAsync<Feedback, int>, 
IGetCountFilteredAsync<Feedback>, IGetByIdAsync<Feedback?, int>,
ICreateAsync<Feedback, int>, IDeleteByIdAsync<int, int?>, IPutAsync<Feedback, int?>, 
Common.Repositories.Base.IIncrementLikesAsync<Feedback, int?>, Common.Repositories.Base.IDecrementLikesAsync<Feedback, int?> 
{
    public Task<decimal> GetAverageRatingAsync(int venueId);
}
