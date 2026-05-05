using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using ApplicationCore.Models;

namespace Infrastructure.Memory;

public class MemoryOrganizationRepository : MemoryGenericRepository<Organization>, IOrganizationRepository
{
    public Task<IEnumerable<Organization>> FindByTypeAsync(OrganizationType type)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Person>> GetMembersAsync(Guid organizationId)
    {
        throw new NotImplementedException();
    }
}
