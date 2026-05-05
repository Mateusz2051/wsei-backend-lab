using System.Threading.Tasks;

namespace ApplicationCore.Interfaces;

public interface IDbSeeder
{
    int Order { get; }
    Task SeedAsync();
}
