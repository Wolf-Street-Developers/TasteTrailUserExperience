using TasteTrailData.Infrastructure.Filters.Dtos;
using TasteTrailUserExperience.Core.Venues.Models;

namespace TasteTrailUserExperience.Core.Venues.Services;

public interface IVenueService
{
    Task<FilterResponseDto<Venue>> GetVenuesFilteredAsync(FilterParametersSearchDto filterParameters);

    Task<Venue?> GetVenueByIdAsync(int id);

    Task<int> GetVenuesCountAsync();
}
