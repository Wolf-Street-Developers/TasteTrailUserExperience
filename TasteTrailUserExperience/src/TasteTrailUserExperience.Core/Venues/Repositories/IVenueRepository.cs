using TasteTrailData.Core.Common.Repositories.Base;
using TasteTrailUserExperience.Core.Venues.Models;

namespace TasteTrailUserExperience.Core.Venues.Repositories;

public interface IVenueRepository : IGetFilteredAsync<Venue>, IGetCountFilteredAsync<Venue>, IGetByIdAsync<Venue?, int>, IGetAsNoTrackingAsync<Venue?, int>, IPutAsync<Venue, int?>, ICreateAsync<Venue, int>, IDeleteByIdAsync<int, int?>
{
}
