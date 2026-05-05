using ApplicationCore.Dto;
using ApplicationCore.Interfaces;

namespace Infrastructure.Memory;

public class CompanyService(IContactUnitOfWork unitOfWork) : ICompanyService
{
    public async Task<IEnumerable<CompanyDto>> GetAllCompanies()
    {
        var companies = await unitOfWork.Companies.FindAllAsync();
        return companies.Select(CompanyDto.FromEntity);
    }
}
