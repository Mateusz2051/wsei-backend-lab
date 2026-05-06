using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Dto;
using ApplicationCore.Models;

namespace ApplicationCore.Interfaces;

public interface IOrganizationService
{
    Task<IEnumerable<OrganizationDto>> GetAllOrganizations();
    Task<OrganizationDto?> GetById(Guid id);
    Task<Organization> AddOrganization(CreateOrganizationDto dto);
    Task DeleteOrganizationAsync(Guid id);
    Task<IEnumerable<PersonDto>> GetMembersAsync(Guid organizationId);
    Task AddMemberAsync(Guid organizationId, Guid personId);
    Task RemoveMemberAsync(Guid organizationId, Guid personId);
}
