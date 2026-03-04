using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Models;
using ApplicationCore.Dto;

namespace ApplicationCore.Interfaces;

public interface IContactRepository : IGenericRepositoryAsync<Contact>
{
    Task<PagedResult<Contact>> SearchAsync(ContactSearchDto searchDto);
    Task<IEnumerable<Contact>> FindByTagAsync(string tagName);
    Task AddNoteAsync(Guid contactId, Note note);
    Task<IEnumerable<Note>> GetNotesAsync(Guid contactId);
    Task AddTagAsync(Guid contactId, Tag tag);
    Task RemoveTagAsync(Guid contactId, Guid tagId);
}
