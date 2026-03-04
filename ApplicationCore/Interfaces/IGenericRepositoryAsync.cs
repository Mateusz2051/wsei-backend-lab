using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Dto;

namespace ApplicationCore.Interfaces;

public interface IGenericRepositoryAsync<T> where T : class
{
    Task<T?> FindByIdAsync(Guid id);
    Task<IEnumerable<T>> FindAllAsync();
    Task<PagedResult<T>> FindPagedAsync(int page, int pageSize);
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task RemoveByIdAsync(Guid id);
}
