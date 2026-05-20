using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Dto;
using ApplicationCore.Models;

namespace ApplicationCore.Interfaces;

public interface IPersonService
{
    Task<PagedResult<PersonDto>> FindAllPeoplePaged(int page, int size);
    Task<IEnumerable<PersonDto>> FindPeopleFromCompany(Guid companyId);

    Task<PersonDto?> GetById(Guid id);

    Task<Person> AddPerson(CreatePersonDto personDto);

    Task<Person> UpdatePerson(Guid id, UpdatePersonDto personDto);

    Task DeletePersonAsync(Guid id);
    Task<Note> AddNoteToPerson(Guid personId, CreateNoteDto noteDto);
    Task<PersonDto?> GetPerson(Guid personId);
    Task AddTagAsync(Guid personId, string tagName);
    Task<IEnumerable<PersonDto>> SearchPeopleAsync(string? emailDomain, Guid? organizationId, Guid? companyId);
}
