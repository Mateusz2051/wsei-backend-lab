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

    public async Task<IEnumerable<Person>> SearchAsync(string? emailDomain, Guid? organizationId, Guid? companyId)
    {
        var query = context.People
            .Include(p => p.Organization)
            .Include(p => p.Employer)
            .Include(p => p.Tags)
            .Include(p => p.Notes)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(emailDomain))
        {
            var domain = emailDomain.Trim();
            if (!domain.StartsWith("@"))
            {
                domain = "@" + domain;
            }
            query = query.Where(p => p.Email != null && p.Email.EndsWith(domain));
        }

        if (organizationId.HasValue)
        {
            query = query.Where(p => p.Organization != null && p.Organization.Id == organizationId.Value);
        }

        if (companyId.HasValue)
        {
            query = query.Where(p => p.Employer != null && p.Employer.Id == companyId.Value);
        }

        return await query.ToListAsync();
    }
}
