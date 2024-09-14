using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TasteTrailUserExperience.Core.Users.Dtos;

public class UserDto
{
    public required string Id { get; set; }

    public required string Role { get; set; }

    public required string Username { get; set; }
}
