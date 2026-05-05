using System.Collections.Generic;
using System.Linq;
using ApplicationCore.Interfaces;

namespace ApplicationCore.Dto;

public record UserDto
{
    public string Id { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Department { get; init; } = string.Empty;
    public SystemUserStatus Status { get; init; }
    public IEnumerable<string> Roles { get; init; } = Enumerable.Empty<string>();
}
