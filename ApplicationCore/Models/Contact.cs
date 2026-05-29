using System;
using System.Collections.Generic;

namespace ApplicationCore.Models;

public abstract class Contact : EntityBase
{
    public string Email { get; set; }
    public string Phone { get; set; }
    public Address Address { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public ContactStatus Status { get; set; }
    public List<Tag> Tags { get; set; } = new();
    public List<Note> Notes { get; set; } = new();

    public string? OwnerId { get; set; }

    public abstract string GetDisplayName();
}
