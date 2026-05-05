using ApplicationCore.Dto;

namespace ApplicationCore.Interfaces;

public interface ICompanyService
{
    Task<IEnumerable<CompanyDto>> GetAllCompanies();
}
