using TasteTrailData.Core.Common.Repositories.Base;
using TasteTrailUserExperience.Core.MenuItems.Models;

namespace TasteTrailUserExperience.Core.MenuItems.Repositories;

public interface IMenuItemRepository : IGetFilteredByIdAsync<MenuItem, int>, IGetFilteredAsync<MenuItem>,IGetCountFilteredIdAsync<MenuItem, int>, IGetCountFilteredAsync<MenuItem>,
IGetByIdAsync<MenuItem?, int>, IGetAsNoTrackingAsync<MenuItem?, int>, IPutAsync<MenuItem, int?>, ICreateAsync<MenuItem, int>, IDeleteByIdAsync<int, int?>, Common.Repositories.Base.IIncrementLikesAsync<MenuItem, int?>, Common.Repositories.Base.IDecrementLikesAsync<MenuItem, int?> 
{

}
