using System;
using System.Threading.Tasks;
using ApplicationCore.Dto;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BackendLab01.Controllers;

[ApiController]
[Route("/api/organizations")]
public class OrganizationsController(IOrganizationService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await service.GetAllOrganizations());
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var organization = await service.GetById(id);
        return organization is null ? NotFound() : Ok(organization);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrganizationDto dto)
    {
        var created = await service.AddOrganization(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, OrganizationDto.FromEntity(created));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await service.DeleteOrganizationAsync(id);
        return NoContent();
    }

    [HttpGet("{id:guid}/members")]
    public async Task<IActionResult> GetMembers(Guid id)
    {
        return Ok(await service.GetMembersAsync(id));
    }

    [HttpPost("{id:guid}/members/{personId:guid}")]
    public async Task<IActionResult> AddMember(Guid id, Guid personId)
    {
        await service.AddMemberAsync(id, personId);
        return NoContent();
    }

    [HttpDelete("{id:guid}/members/{personId:guid}")]
    public async Task<IActionResult> RemoveMember(Guid id, Guid personId)
    {
        await service.RemoveMemberAsync(id, personId);
        return NoContent();
    }
}
