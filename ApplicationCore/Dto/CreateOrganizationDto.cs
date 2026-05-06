using ApplicationCore.Models;

namespace ApplicationCore.Dto;

public record CreateOrganizationDto(
    string Name,
    OrganizationType Type,
    string? Krs,
    string? Website,
    string? Mission,
    string Email,
    string Phone,
    AddressDto? Address
);
