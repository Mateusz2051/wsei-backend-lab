using System.Threading.Tasks;
using ApplicationCore.Interfaces;

namespace Infrastructure.Memory;

public class MemoryContactUnitOfWork(
    IPersonRepository persons,
    ICompanyRepository companies,
    IOrganizationRepository organizations
) : IContactUnitOfWork
{
    public IPersonRepository Persons => persons;
    public ICompanyRepository Companies => companies;
    public IOrganizationRepository Organizations => organizations;

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;

    public Task<int> SaveChangesAsync() => Task.FromResult(0);

    public Task BeginTransactionAsync() => Task.CompletedTask;

    public Task CommitTransactionAsync() => Task.CompletedTask;

    public Task RollbackTransactionAsync() => Task.CompletedTask;
}
