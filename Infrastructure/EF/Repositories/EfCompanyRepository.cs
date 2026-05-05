using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Infrastructure.EF.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EF.Repositories;

public class EfCompanyRepository(ContactsDbContext context) : 
    EfGenericRepository<Company>(context.Companies), 
    ICompanyRepository
{
    public Task<Company?> FindByNipAsync(string nip)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Company>> FindByNameAsync(string name)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Person>> GetEmployeesAsync(Guid companyId)
    {
        var result = context.People.Where(p => p.Employer != null && p.Employer.Id == companyId).AsEnumerable();
        return Task.FromResult(result);
    }
}
