using TasteTrailData.Core.Common.Repositories.Base;
using TasteTrailUserExperience.Core.Menus.Models;

namespace TasteTrailUserExperience.Core.Menus.Repositories;

public interface IMenuRepository : IGetFilteredByIdAsync<Menu, int>, IGetFilteredAsync<Menu>, 
    IGetCountFilteredIdAsync<Menu, int>, IGetCountFilteredAsync<Menu>, IGetByIdAsync<Menu?, int>,
    IPutAsync<Menu, int?>, ICreateAsync<Menu, int>, IDeleteByIdAsync<int, int?>
{
    
}
