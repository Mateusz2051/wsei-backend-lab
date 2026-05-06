using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Dto;
using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces;
using ApplicationCore.Models;

namespace Infrastructure.Memory;

public class OrganizationService(IContactUnitOfWork unitOfWork) : IOrganizationService
{
    public async Task<IEnumerable<OrganizationDto>> GetAllOrganizations()
    {
        var organizations = await unitOfWork.Organizations.FindAllAsync();
        return organizations.Select(OrganizationDto.FromEntity);
    }

    public async Task<OrganizationDto?> GetById(Guid id)
    {
        var organization = await unitOfWork.Organizations.FindByIdAsync(id);
        return organization is null ? null : OrganizationDto.FromEntity(organization);
    }

    public async Task<Organization> AddOrganization(CreateOrganizationDto dto)
    {
        var entity = OrganizationDto.ToEntity(dto);
        entity = await unitOfWork.Organizations.AddAsync(entity);
        await unitOfWork.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteOrganizationAsync(Guid id)
    {
        var organization = await unitOfWork.Organizations.FindByIdAsync(id)
            ?? throw new ContactNotFoundException($"Organization with id={id} not found!");

        var members = await unitOfWork.Persons.FindByOrganizationAsync(id);
        foreach (var member in members)
        {
            member.Organization = null;
            await unitOfWork.Persons.UpdateAsync(member);
        }

        await unitOfWork.Organizations.RemoveByIdAsync(organization.Id);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<PersonDto>> GetMembersAsync(Guid organizationId)
    {
        var members = await unitOfWork.Persons.FindByOrganizationAsync(organizationId);
        return members.Select(PersonDto.FromEntity);
    }

    public async Task AddMemberAsync(Guid organizationId, Guid personId)
    {
        var organization = await unitOfWork.Organizations.FindByIdAsync(organizationId)
            ?? throw new ContactNotFoundException($"Organization with id={organizationId} not found!");

        var person = await unitOfWork.Persons.FindByIdAsync(personId)
            ?? throw new ContactNotFoundException($"Person with id={personId} not found!");

        person.Organization = organization;

        if (organization.Members.All(m => m.Id != person.Id))
        {
            organization.Members.Add(person);
        }

        await unitOfWork.Persons.UpdateAsync(person);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task RemoveMemberAsync(Guid organizationId, Guid personId)
    {
        var organization = await unitOfWork.Organizations.FindByIdAsync(organizationId)
            ?? throw new ContactNotFoundException($"Organization with id={organizationId} not found!");

        var person = await unitOfWork.Persons.FindByIdAsync(personId)
            ?? throw new ContactNotFoundException($"Person with id={personId} not found!");

        if (person.Organization?.Id != organization.Id)
        {
            throw new ContactNotFoundException(
                $"Person with id={personId} is not a member of organization id={organizationId}!");
        }

        person.Organization = null;
        organization.Members.RemoveAll(m => m.Id == person.Id);

        await unitOfWork.Persons.UpdateAsync(person);
        await unitOfWork.SaveChangesAsync();
    }
}
