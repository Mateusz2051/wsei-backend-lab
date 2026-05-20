using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Dto;
using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces;
using ApplicationCore.Models;

namespace Infrastructure.Memory;

public class MemoryPersonService(IContactUnitOfWork unitOfWork) : IPersonService
{
    public async Task<PagedResult<PersonDto>> FindAllPeoplePaged(int page, int size)
    {
        var paged = await unitOfWork.Persons.FindPagedAsync(page, size);
        var items = paged.Items.Select(PersonDto.FromEntity).ToList();
        return new PagedResult<PersonDto>(items, paged.TotalCount, paged.Page, paged.PageSize);
    }

    public async Task<IEnumerable<PersonDto>> FindPeopleFromCompany(Guid companyId)
    {
        var people = await unitOfWork.Persons.FindByCompanyAsync(companyId);
        return people.Select(PersonDto.FromEntity);
    }

    public async Task<PersonDto?> GetById(Guid id)
    {
        var person = await unitOfWork.Persons.FindByIdAsync(id);
        return person is null ? null : PersonDto.FromEntity(person);
    }

    public async Task<Person> AddPerson(CreatePersonDto personDto)
    {
        var entity = PersonDto.ToEntity(personDto);
        if (personDto.EmployerId.HasValue)
        {
            entity.Employer = await unitOfWork.Companies.FindByIdAsync(personDto.EmployerId.Value);
        }
        entity = await unitOfWork.Persons.AddAsync(entity);
        await unitOfWork.SaveChangesAsync();
        return entity;
    }

    public async Task<Person> UpdatePerson(Guid id, UpdatePersonDto personDto)
    {
        var person = await unitOfWork.Persons.FindByIdAsync(id)
            ?? throw new KeyNotFoundException($"Person with id {id} not found.");

        PersonDto.ApplyUpdate(person, personDto);
        if (personDto.EmployerId is not null)
        {
            person.Employer = personDto.EmployerId.Value == Guid.Empty
                ? null
                : await unitOfWork.Companies.FindByIdAsync(personDto.EmployerId.Value);
        }
        var updated = await unitOfWork.Persons.UpdateAsync(person);
        await unitOfWork.SaveChangesAsync();
        return updated;
    }

    public async Task DeletePersonAsync(Guid id)
    {
        await unitOfWork.Persons.RemoveByIdAsync(id);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task<PersonDto?> GetPerson(Guid personId)
    {
        return await GetById(personId);
    }

    public async Task<Note> AddNoteToPerson(Guid personId, CreateNoteDto noteDto)
    {
        var person = await unitOfWork.Persons.FindByIdAsync(personId)
            ?? throw new ContactNotFoundException($"Person with id={personId} not found!");

        if (person.Notes is null)
        {
            person.Notes = new List<Note>();
        }

        var note = new Note
        {
            Id = Guid.NewGuid(),
            Content = noteDto.Content,
            CreatedAt = DateTime.UtcNow
        };

        person.Notes.Add(note);

        await unitOfWork.Persons.UpdateAsync(person);
        await unitOfWork.SaveChangesAsync();
        
        return note;
    }

    public async Task AddTagAsync(Guid personId, string tagName)
    {
        var person = await unitOfWork.Persons.FindByIdAsync(personId)
            ?? throw new KeyNotFoundException($"Person with id {personId} not found.");

        person.Tags.Add(new Tag
        {
            Id = Guid.NewGuid(),
            Name = tagName
        });

        await unitOfWork.Persons.UpdateAsync(person);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<PersonDto>> SearchPeopleAsync(string? emailDomain, Guid? organizationId, Guid? companyId)
    {
        var people = await unitOfWork.Persons.SearchAsync(emailDomain, organizationId, companyId);
        return people.Select(PersonDto.FromEntity);
    }
}
