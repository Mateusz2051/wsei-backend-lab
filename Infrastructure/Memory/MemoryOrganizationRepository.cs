using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using ApplicationCore.Models;

namespace Infrastructure.Memory;

public class MemoryOrganizationRepository : MemoryGenericRepository<Organization>, IOrganizationRepository
{
    public Task<IEnumerable<Organization>> FindByTypeAsync(OrganizationType type)
    {
        var result = _data.Values.Where(o => o.Type == type);
        return Task.FromResult(result);
    }

    public Task<IEnumerable<Person>> GetMembersAsync(Guid organizationId)
    {
        var organization = _data.Values.FirstOrDefault(o => o.Id == organizationId);
        var members = organization?.Members ?? new List<Person>();
        return Task.FromResult(members.AsEnumerable());
    }
}
