using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationCore.Models;

namespace ApplicationCore.Dto;

public abstract record ContactBaseDto
{
    public Guid Id { get; init; }
    public string Email { get; init; }
    public string Phone { get; init; }
    public AddressDto Address { get; init; }
    public ContactStatus Status { get; init; }
    public List<string> Tags { get; init; } = new();
    public List<Note> Notes { get; init; } = new();
    public DateTime CreatedAt { get; init; }

    protected static TDto MapFromContact<TDto>(Contact contact, TDto dto) where TDto : ContactBaseDto
        => dto;

    public static AddressDto? MapAddress(Address? address) =>
        address is null ? null : new AddressDto(
            address.Street,
            address.City,
            address.PostalCode,
            address.Country,
            address.Type
        );

    public static Address? MapAddress(AddressDto? dto) =>
        dto is null ? null : new Address
        {
            Street = dto.Street,
            City = dto.City,
            PostalCode = dto.PostalCode,
            Country = dto.Country,
            Type = dto.Type
        };
}
