using ApplicationCore.Models;

namespace ApplicationCore.Dto;

public record CompanyDto : ContactBaseDto
{
    public string Name { get; init; }
    public string Industry { get; init; }
    public string Website { get; init; }

    public static CompanyDto FromEntity(Company company) => new()
    {
        Id = company.Id,
        Name = company.Name,
        Industry = company.Industry,
        Website = company.Website,
        Email = company.Email,
        Phone = company.Phone,
        Status = company.Status,
        CreatedAt = company.CreatedAt,
        Address = MapAddress(company.Address)
    };
}
