using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Dto;
using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EF.Repositories;

public class EfGenericRepository<T>(DbSet<T> set) : IGenericRepositoryAsync<T> where T : EntityBase
{
    public async Task<T?> FindByIdAsync(Guid id)
    {
        return await set.FindAsync(id);
    }

    public async Task<IEnumerable<T>> FindAllAsync()
    {
        return await set.ToListAsync();
    }

    public async Task<PagedResult<T>> FindPagedAsync(int page, int pageSize)
    {
        var items = await set
            .AsNoTracking()
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
            
        var totalCount = await set.CountAsync();
        
        return new PagedResult<T>(items, totalCount, page, pageSize);
    }

    public async Task<T> AddAsync(T entity)
    {
        var entry = await set.AddAsync(entity);
        return entry.Entity;
    }

    public Task<T> UpdateAsync(T entity)
    {
        var entityEntry = set.Update(entity);
        return Task.FromResult(entityEntry.Entity);
    }

    public Task RemoveByIdAsync(Guid id)
    {
        var entity = set.Find(id);
        if (entity is not null)
        {
            set.Remove(entity);
        }
        return Task.CompletedTask;
    }
}
