using System;

namespace ApplicationCore.Dto;

public record CreateNoteDto
{
    public string Content { get; init; }
}
