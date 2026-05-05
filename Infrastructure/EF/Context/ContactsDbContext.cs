using System;
using ApplicationCore.Models;
using Infrastructure.EF.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EF.Context;

public class ContactsDbContext : IdentityDbContext<CrmUser, CrmRole, string>
{
    public DbSet<Person> People { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<Infrastructure.Security.RefreshToken> RefreshTokens { get; set; }

    public ContactsDbContext()
    {
    }

    public ContactsDbContext(DbContextOptions<ContactsDbContext> options) :
        base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder); 
        
        builder.Entity<CrmUser>(entity =>
        {
            entity.Property(u => u.FirstName).HasMaxLength(100);
            entity.Property(u => u.LastName).HasMaxLength(100);
            entity.Property(u => u.Department).HasMaxLength(100);
            entity.HasIndex(u => u.Email).IsUnique();
        });
        
        builder.Entity<CrmRole>(entity =>
        {
            entity.Property(r => r.Name).HasMaxLength(20);
        });
        
        // Konfiguracji mapowania dziedziczenia TPH
        // Jedna tabela do przechowywnia wszystkich typów kontaktów
        builder.Entity<Contact>()
            .HasDiscriminator<string>("ContactType")
            .HasValue<Person>("Person")
            .HasValue<Company>("Company")
            .HasValue<Organization>("Organization");

        builder.Entity<Contact>(entity =>
        {
            entity.Property(p => p.Email).HasMaxLength(200);
            entity.Property(p => p.Phone).HasMaxLength(20);
        });
        
        builder.Entity<Person>(entity =>
        {
            entity.Property(p => p.BirthDate).HasColumnType("TEXT"); 
            entity.Property(p => p.Gender).HasConversion<string>();
            entity.Property(p => p.Status).HasConversion<string>();
        });
        
        // definicja związku  
        builder.Entity<Person>()
            .HasOne(p => p.Employer)
            .WithMany(e => e.Employees);
     
     // definicja związku  
        builder.Entity<Organization>()
            .HasMany(o => o.Members)
            .WithOne(p => p.Organization);
        
        // przykładowa firma
        builder.Entity<Company>(entity =>
        {
            entity.HasData(
                new Company
                {
                    Id = Guid.Parse("516A34D7-CCFB-4F20-85F3-62BD0F3AF271"),
                    Name = "WSEI",
                    Industry = "Edukacja",
                    Phone = "123567123",
                    Email = "biuro@wsei.edu.pl",
                    Website = "https://wsei.edu.pl",
                }
            );
        });
        
        var address1 = new
        {
            City = "Kraków",
            Country = "Poland",
            PostalCode = "31-150",
            Street = "ul. Św. Filipa 17",
            Type = AddressType.Correspondence,
            ContactId = Guid.Parse("3d54091d-abc8-49ec-9590-93ad3ed5458f")
        };

        var address2 = new
        {
            City = "Warszawa",
            Country = "Poland",
            PostalCode = "00-001",
            Street = "ul. Wiejska 1",
            Type = AddressType.Correspondence,
            ContactId = Guid.Parse("B4DCB17C-F875-43F8-9D66-36597895A466")
        };

        var address3 = new
        {
            City = "Kraków",
            Country = "Poland",
            PostalCode = "31-150",
            Street = "ul. Św. Filipa 17",
            Type = AddressType.Correspondence,
            ContactId = Guid.Parse("516A34D7-CCFB-4F20-85F3-62BD0F3AF271")
        };
        
        builder.Entity<Person>(entity =>
        {
            entity.HasData(
                new
                {
                    Id = Guid.Parse("3d54091d-abc8-49ec-9590-93ad3ed5458f"),
                    FirstName = "Adam",
                    LastName = "Nowak",
                    Gender = Gender.Male,
                    Status = ContactStatus.Active,
                    Email = "adam@wsei.edu.pl",
                    Phone = "123456789",
                    BirthDate = DateTime.Parse("2001-01-11"),
                    Position = "Programista",
                    CreatedAt = DateTime.UtcNow
                },
                new 
                {
                    Id = Guid.Parse("B4DCB17C-F875-43F8-9D66-36597895A466"),
                    FirstName = "Ewa",
                    LastName = "Kowalska",
                    Gender = Gender.Female,
                    Status = ContactStatus.Blocked,
                    Email = "ewa@wsei.edu.pl",
                    Phone = "123123123",
                    BirthDate = DateTime.Parse("2001-01-11"),
                    Position = "Tester",
                    CreatedAt = DateTime.UtcNow
                });
        });
        
        builder.Entity<Contact>()
            .OwnsOne(c => c.Address)
            .HasData(address1, address2, address3);
    }
}
