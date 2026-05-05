using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Infrastructure.EF.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EF.Repositories;

public class EfPersonRepository(ContactsDbContext context) : 
    EfGenericRepository<Person>(context.People), 
    IPersonRepository
{
    public async Task<IEnumerable<Person>> FindByCompanyAsync(Guid companyId)
    {
        return await context.People
            .Where(p => p.Employer != null && p.Employer.Id == companyId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Person>> FindByOrganizationAsync(Guid organizationId)
    {
        return await context.People
            .Where(p => p.Organization != null && p.Organization.Id == organizationId)
            .ToListAsync();
    }
}
