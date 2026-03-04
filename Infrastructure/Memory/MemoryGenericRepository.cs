using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Models;
using ApplicationCore.Interfaces;
using ApplicationCore.Dto;

namespace Infrastructure.Memory;

public class MemoryGenericRepository<T> : IGenericRepositoryAsync<T> 
    where T : EntityBase 
{
    protected readonly Dictionary<Guid, T> _data = new();
    
    public Task<T?> FindByIdAsync(Guid id)
    {
        var result = _data.TryGetValue(id, out var value) ? value : null;
        return Task.FromResult(result);
    }

    public Task<IEnumerable<T>> FindAllAsync()
    {
        return Task.FromResult(_data.Values.AsEnumerable());
    }

    public Task<PagedResult<T>> FindPagedAsync(int page, int pageSize)
    {
        var items = _data.Values.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        var totalCount = _data.Count;
        var pagedResult = new PagedResult<T>(items, totalCount, page, pageSize);
        return Task.FromResult(pagedResult);
    }

    public Task<T> AddAsync(T entity)
    {
        if (entity.Id == Guid.Empty)
        {
            entity.Id = Guid.NewGuid();
        }
        _data[entity.Id] = entity;
        return Task.FromResult(entity);
    }

    public Task<T> UpdateAsync(T entity)
    {
        if (_data.ContainsKey(entity.Id))
        {
            _data[entity.Id] = entity;
            return Task.FromResult(entity);
        }
        throw new KeyNotFoundException($"Entity with Id {entity.Id} not found.");
    }

    public Task RemoveByIdAsync(Guid id)
    {
        if (_data.ContainsKey(id))
        {
            _data.Remove(id);
        }
        else
        {
            throw new KeyNotFoundException($"Entity with Id {id} not found.");
        }
        return Task.CompletedTask;
    }
}
