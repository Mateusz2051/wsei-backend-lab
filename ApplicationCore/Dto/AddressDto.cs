using ApplicationCore.Models;

namespace ApplicationCore.Dto;

public record AddressDto(
    string Street,
    string City,
    string PostalCode,
    string Country,
    AddressType Type
);
