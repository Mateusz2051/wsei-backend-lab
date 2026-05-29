using System;
using System.Linq;
using ApplicationCore.Models;

namespace ApplicationCore.Dto;

public record PersonDto : ContactBaseDto
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string? Position { get; init; }
    public DateTime? BirthDate { get; init; }
    public Gender Gender { get; init; }
    public Guid? EmployerId { get; init; }
    public Guid? OrganizationId { get; init; }
    public string? Pesel { get; init; }
    public string? OwnerId { get; init; }

    public static PersonDto FromEntity(Person person) => new()
    {
        Id = person.Id,
        FirstName = person.FirstName,
        LastName = person.LastName,
        Email = person.Email,
        Phone = person.Phone,
        Position = person.Position,
        BirthDate = person.BirthDate,
        Gender = person.Gender,
        EmployerId = person.Employer?.Id,
        OrganizationId = person.Organization?.Id,
        Pesel = person.Pesel?.Value,
        OwnerId = person.OwnerId,
        Status = person.Status,
        Tags = person.Tags.Select(t => t.Name).ToList(),
        Notes = person.Notes,
        CreatedAt = person.CreatedAt,
        Address = MapAddress(person.Address)
    };

    public static Person ToEntity(CreatePersonDto dto) => new()
    {
        Id = Guid.NewGuid(),
        FirstName = dto.FirstName,
        LastName = dto.LastName,
        Email = dto.Email,
        Phone = dto.Phone,
        Position = dto.Position,
        BirthDate = dto.BirthDate,
        Gender = dto.Gender,
        CreatedAt = DateTime.UtcNow,
        Status = ContactStatus.Active,
        Address = MapAddress(dto.Address),
        Pesel = dto.Pesel is not null ? new Pesel(dto.Pesel) : null,
        OwnerId = dto.OwnerId
    };

    public static Person ApplyUpdate(Person person, UpdatePersonDto dto)
    {
        if (dto.FirstName is not null) person.FirstName = dto.FirstName;
        if (dto.LastName is not null) person.LastName = dto.LastName;
        if (dto.Email is not null) person.Email = dto.Email;
        if (dto.Phone is not null) person.Phone = dto.Phone;
        if (dto.Position is not null) person.Position = dto.Position;
        if (dto.BirthDate is not null) person.BirthDate = dto.BirthDate;
        if (dto.Gender is not null) person.Gender = dto.Gender.Value;
        if (dto.Status is not null) person.Status = dto.Status.Value;
        if (dto.Address is not null) person.Address = MapAddress(dto.Address);
        if (dto.Pesel is not null) person.Pesel = string.IsNullOrWhiteSpace(dto.Pesel) ? null : new Pesel(dto.Pesel);
        person.UpdatedAt = DateTime.UtcNow;
        return person;
    }
}
