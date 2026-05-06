using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Infrastructure.EF.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EF.Repositories;

public class EfOrganizationRepository(ContactsDbContext context) : 
    EfGenericRepository<Organization>(context.Organizations), 
    IOrganizationRepository
{
    public async Task<IEnumerable<Organization>> FindByTypeAsync(OrganizationType type)
    {
        return await context.Organizations
            .Where(o => o.Type == type)
            .ToListAsync();
    }

    public async Task<IEnumerable<Person>> GetMembersAsync(Guid organizationId)
    {
        return await context.People
            .Where(p => p.Organization != null && p.Organization.Id == organizationId)
            .ToListAsync();
    }
}
