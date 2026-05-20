using ApplicationCore.Authorization;
using ApplicationCore.Dto;
using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendLab01.Controllers;

[ApiController]
[Route("/api/contacts")]
public class ContactsController(IPersonService service) : ControllerBase
{
    [HttpGet]
    [Authorize(Policy = nameof(CrmPolicies.ReadOnlyAccess))]
    public async Task<IActionResult> GetAllPersons([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        return Ok(await service.FindAllPeoplePaged(page, size));
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchPeople(
        [FromQuery] string? emailDomain,
        [FromQuery] Guid? organizationId,
        [FromQuery] Guid? companyId)
    {
        var results = await service.SearchPeopleAsync(emailDomain, organizationId, companyId);
        return Ok(results);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetPerson(Guid id)
    {
        var dto = await service.GetById(id);
        if (dto is null)
            return NotFound();
        return Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePersonDto dto)
    {
        var result = await service.AddPerson(dto);
        return CreatedAtAction(nameof(GetPerson), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePersonDto dto)
    {
        var existing = await service.GetById(id);
        if (existing is null)
            return NotFound();

        var updated = await service.UpdatePerson(id, dto);
        return Ok(PersonDto.FromEntity(updated));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var existing = await service.GetById(id);
        if (existing is null)
            return NotFound();

        await service.DeletePersonAsync(id);
        return NoContent();
    }

    [HttpPost("{contactId:guid}/notes")]
    [ProducesResponseType(typeof(Note), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddNote(
        [FromRoute] Guid contactId,
        [FromBody] CreateNoteDto dto)
    {
        var note = await service.AddNoteToPerson(contactId, dto);
        return CreatedAtAction(
            nameof(GetNotes),
            new { contactId },
            note);
    }

    [HttpGet("{contactId:guid}/notes")]
    [ProducesResponseType(typeof(IEnumerable<Note>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetNotes([FromRoute] Guid contactId)
    {
        var person = await service.GetPerson(contactId);
        if (person is null)
        {
            return NotFound();
        }
        return Ok(person.Notes);
    }

    [HttpPost("{id:guid}/tags")]
    public async Task<IActionResult> AddTag(Guid id, [FromBody] AddTagRequest request)
    {
        var existing = await service.GetById(id);
        if (existing is null)
            return NotFound();

        await service.AddTagAsync(id, request.TagName);
        return NoContent();
    }
}

public record AddTagRequest(string TagName);
