using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using ApplicationCore.Models;

namespace Infrastructure.Memory;

public class MemoryPersonRepository : MemoryGenericRepository<Person>, IPersonRepository
{
    public MemoryPersonRepository() : base()
    {
        _data.Add(Guid.NewGuid(), new Person
        {
            FirstName = "Adam",
            LastName = "Nowak",
            Gender = Gender.Male,
            Email = "adam.nowak@example.com",
            Phone = "600-100-200",
            Status = ContactStatus.Active,
            CreatedAt = DateTime.UtcNow
        });

        _data.Add(Guid.NewGuid(), new Person
        {
            FirstName = "Anna",
            LastName = "Kowalska",
            Gender = Gender.Female,
            Email = "anna.kowalska@example.com",
            Phone = "700-200-300",
            Status = ContactStatus.Active,
            CreatedAt = DateTime.UtcNow
        });
    }

    public Task<IEnumerable<Person>> FindByCompanyAsync(Guid companyId)
    {
        var result = _data.Values.Where(p => p.Employer != null && p.Employer.Id == companyId);
        return Task.FromResult(result);
    }

    public Task<IEnumerable<Person>> FindByOrganizationAsync(Guid organizationId)
    {
        var result = _data.Values.Where(p => p.Organization != null && p.Organization.Id == organizationId);
        return Task.FromResult(result);
    }
}
