using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using Infrastructure.EF.Context;

namespace Infrastructure.EF.UnitOfWork;

public class EfContactsUnitOfWork(
    IPersonRepository personRepository,
    ICompanyRepository companyRepository,
    IOrganizationRepository organizationRepository,
    ContactsDbContext context
    ) : IContactUnitOfWork
{
    public ValueTask DisposeAsync()
    {
        return context.DisposeAsync();
    }

    public IPersonRepository Persons => personRepository;
    public ICompanyRepository Companies => companyRepository;
    public IOrganizationRepository Organizations => organizationRepository;

    public Task<int> SaveChangesAsync()
    {
        return context.SaveChangesAsync();
    }

    public Task BeginTransactionAsync()
    {
        return context.Database.BeginTransactionAsync();
    }

    public Task CommitTransactionAsync()
    {
        return context.Database.CommitTransactionAsync();
    }

    public Task RollbackTransactionAsync()
    {
        return context.Database.RollbackTransactionAsync();
    }
}
