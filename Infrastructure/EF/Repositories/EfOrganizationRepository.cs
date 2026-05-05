using System;
using System.Collections.Generic;
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
    public Task<IEnumerable<Organization>> FindByTypeAsync(OrganizationType type)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Person>> GetMembersAsync(Guid organizationId)
    {
        throw new NotImplementedException();
    }
}
