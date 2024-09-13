using TasteTrailData.Core.Filters.Specifications;
using TasteTrailData.Infrastructure.Filters.Dtos;
using TasteTrailUserExperience.Core.Venues.Models;
using TasteTrailUserExperience.Core.Venues.Repositories;
using TasteTrailUserExperience.Core.Venues.Services;
using TasteTrailUserExperience.Infrastructure.Venues.Factories;

namespace TasteTrailUserExperience.Infrastructure.Venues.Services;

public class VenueService : IVenueService
{
    private readonly IVenueRepository _venueRepository;

    public VenueService(IVenueRepository venueRepository)
    {
        _venueRepository = venueRepository;
    }

    public async Task<FilterResponseDto<Venue>> GetVenuesFilteredAsync(FilterParametersSearchDto filterParameters)
    {
        var newFilterParameters = new FilterParameters<Venue>() {
            PageNumber = filterParameters.PageNumber,
            PageSize = filterParameters.PageSize,
            Specification = VenueFilterFactory.CreateFilter(filterParameters.Type),
            SearchTerm = filterParameters.SearchTerm
        };

        var venues = await _venueRepository.GetFilteredAsync(newFilterParameters);

        var totalVenues = await _venueRepository.GetCountFilteredAsync(newFilterParameters);
        var totalPages = (int)Math.Ceiling(totalVenues / (double)filterParameters.PageSize);

        var filterReponse = new FilterResponseDto<Venue>() {
            CurrentPage = filterParameters.PageNumber,
            AmountOfPages = totalPages,
            AmountOfEntities = totalVenues,
            Entities = venues,
        };

        return filterReponse;
    }

    public async Task<Venue?> GetVenueByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException($"Invalid ID value: {id}.");

        var venue = await _venueRepository.GetByIdAsync(id);
        return venue;
    }

    public Task<int> GetVenuesCountAsync()
    {
        return _venueRepository.GetCountFilteredAsync(null);
    }
}
