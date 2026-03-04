using System;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Models;
using ApplicationCore.Interfaces;
using Infrastructure.Memory;
using Xunit;

namespace App.UnitTests;

public class MemoryGenericRepositoryTest
{
    private readonly IGenericRepositoryAsync<Person> _repo = new MemoryGenericRepository<Person>();

    [Fact]
    public async Task AddPersonTestAsync()
    {
        var expected = new Person()
        {
            FirstName = "Adam",
            LastName = "Kowalski"
        };
        await _repo.AddAsync(expected);
        var actual = await _repo.FindByIdAsync(expected.Id);
        Assert.Equal(expected, actual);
        Assert.Equal(expected.Id, actual?.Id);
    }

    [Fact]
    public async Task FindByIdAsync_WhenPersonDoesNotExist_ShouldReturnNull()
    {
        var result = await _repo.FindByIdAsync(Guid.NewGuid());
        Assert.Null(result);
    }

    [Fact]
    public async Task FindAllAsync_ShouldReturnAllAddedPersons()
    {
        var p1 = new Person { FirstName = "A" };
        var p2 = new Person { FirstName = "B" };
        await _repo.AddAsync(p1);
        await _repo.AddAsync(p2);

        var result = await _repo.FindAllAsync();
        Assert.Contains(p1, result);
        Assert.Contains(p2, result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task FindPagedAsync_ShouldReturnPagedResults()
    {
        for (int i = 0; i < 5; i++)
        {
            await _repo.AddAsync(new Person { FirstName = $"Test{i}" });
        }

        var result = await _repo.FindPagedAsync(page: 2, pageSize: 2);
        Assert.Equal(2, result.Items.Count);
        Assert.Equal(5, result.TotalCount);
        Assert.Equal(3, result.TotalPages);
        Assert.True(result.HasNext);
        Assert.True(result.HasPrevious);
    }

    [Fact]
    public async Task UpdateAsync_WhenPersonExists_ShouldUpdatePerson()
    {
        var person = new Person { FirstName = "OldName" };
        await _repo.AddAsync(person);

        person.FirstName = "NewName";
        await _repo.UpdateAsync(person);

        var updated = await _repo.FindByIdAsync(person.Id);
        Assert.Equal("NewName", updated?.FirstName);
    }

    [Fact]
    public async Task UpdateAsync_WhenPersonDoesNotExist_ShouldThrowKeyNotFoundException()
    {
        var person = new Person { Id = Guid.NewGuid(), FirstName = "Ghost" };
        await Assert.ThrowsAsync<System.Collections.Generic.KeyNotFoundException>(() => _repo.UpdateAsync(person));
    }

    [Fact]
    public async Task RemoveByIdAsync_WhenPersonExists_ShouldRemovePerson()
    {
        var person = new Person { FirstName = "ToBeRemoved" };
        await _repo.AddAsync(person);

        await _repo.RemoveByIdAsync(person.Id);

        var result = await _repo.FindByIdAsync(person.Id);
        Assert.Null(result);
    }

    [Fact]
    public async Task RemoveByIdAsync_WhenPersonDoesNotExist_ShouldThrowKeyNotFoundException()
    {
        await Assert.ThrowsAsync<System.Collections.Generic.KeyNotFoundException>(() => _repo.RemoveByIdAsync(Guid.NewGuid()));
    }
}
