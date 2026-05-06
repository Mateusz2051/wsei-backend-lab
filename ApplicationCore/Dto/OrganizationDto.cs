using System;
using ApplicationCore.Models;

namespace ApplicationCore.Dto;

public record OrganizationDto : ContactBaseDto
{
    public string Name { get; init; } = string.Empty;
    public OrganizationType Type { get; init; }
    public string? Krs { get; init; }
    public string? Website { get; init; }
    public string? Mission { get; init; }
    public Guid? PrimaryContactId { get; init; }

    public static OrganizationDto FromEntity(Organization organization) => new()
    {
        Id = organization.Id,
        Name = organization.Name,
        Type = organization.Type,
        Krs = organization.KRS,
        Website = organization.Website,
        Mission = organization.Mission,
        PrimaryContactId = organization.PrimaryContact?.Id,
        Email = organization.Email,
        Phone = organization.Phone,
        Status = organization.Status,
        CreatedAt = organization.CreatedAt,
        Address = MapAddress(organization.Address)
    };

    public static Organization ToEntity(CreateOrganizationDto dto) => new()
    {
        Id = Guid.NewGuid(),
        Name = dto.Name,
        Type = dto.Type,
        KRS = dto.Krs,
        Website = dto.Website,
        Mission = dto.Mission,
        Email = dto.Email,
        Phone = dto.Phone,
        Status = ContactStatus.Active,
        CreatedAt = DateTime.UtcNow,
        Address = MapAddress(dto.Address)
    };
}
