using TasteTrailUserExperience.Core.Feedbacks.Models;

namespace TasteTrailUserExperience.Core.Common.Models;

public interface IFeedbackable
{
    ICollection<Feedback> Feedbacks { get; set; }
}
