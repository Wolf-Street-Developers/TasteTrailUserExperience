using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TasteTrailData.Core.Common.Repositories.Base;
using TasteTrailUserExperience.Core.Users.Models;
namespace TasteTrailUserExperience.Core.Users.Repositories;

public interface IUserRepository : IGetByIdAsync<User, string?>, ICreateAsync<User, string>, IPutAsync<User, string?>
{
    
}
