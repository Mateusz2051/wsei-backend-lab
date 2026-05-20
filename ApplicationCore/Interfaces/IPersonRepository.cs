using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Models;

namespace ApplicationCore.Interfaces;

public interface IPersonRepository : IGenericRepositoryAsync<Person>
{
    Task<IEnumerable<Person>> FindByCompanyAsync(Guid companyId);
    Task<IEnumerable<Person>> FindByOrganizationAsync(Guid organizationId);
    Task<IEnumerable<Person>> SearchAsync(string? emailDomain, Guid? organizationId, Guid? companyId);
}
