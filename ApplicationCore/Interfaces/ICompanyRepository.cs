using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Models;

namespace ApplicationCore.Interfaces;

public interface ICompanyRepository : IGenericRepositoryAsync<Company>
{
    Task<IEnumerable<Company>> FindByNameAsync(string namePart);
    Task<Company?> FindByNipAsync(string nip);
    Task<IEnumerable<Person>> GetEmployeesAsync(Guid companyId);
}
