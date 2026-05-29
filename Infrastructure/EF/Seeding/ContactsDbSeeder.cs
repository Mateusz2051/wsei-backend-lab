using System;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Infrastructure.EF.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.EF.Seeding;

public class ContactsDbSeeder : IDbSeeder
{
    private readonly ContactsDbContext _dbContext;
    private readonly ILogger<ContactsDbSeeder> _logger;

    public ContactsDbSeeder(ContactsDbContext dbContext, ILogger<ContactsDbSeeder> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public int Order => 2; // Po IdentityDbSeeder

    public async Task SeedAsync()
    {
        if (await _dbContext.Organizations.AnyAsync())
        {
            _logger.LogInformation("Zasoby kontaktów są już zainicjowane - pomijam ContactsDbSeeder.");
            return;
        }

        var organizationId = Guid.Parse("A1234567-89AB-CDEF-0123-456789ABCDEF");
        var adminId = "F5BADE14-6CC8-42A2-9A44-9842DA2D9280"; // Zgodnie z IdentityDbSeeder

        var organization = new Organization
        {
            Id = organizationId,
            Name = "Koło Naukowe IT WSEI",
            Phone = "987654321",
            Email = "kolonaukowe@wsei.edu.pl",
            CreatedAt = DateTime.UtcNow,
            OwnerId = adminId
        };

        var memberId = Guid.Parse("B1234567-89AB-CDEF-0123-456789ABCDEF");
        var person = new Person
        {
            Id = memberId,
            FirstName = "Krzysztof",
            LastName = "Krawczyk",
            Gender = Gender.Male,
            Email = "krzysztof.krawczyk@wsei.edu.pl",
            Phone = "555666777",
            BirthDate = new DateTime(1999, 10, 15),
            Pesel = new Pesel("99101512345"),
            Organization = organization,
            CreatedAt = DateTime.UtcNow,
            OwnerId = adminId
        };

        var address1 = new Address
        {
            City = "Kraków",
            Country = "Polska",
            PostalCode = "31-150",
            Street = "ul. Św. Filipa 17",
            Type = AddressType.Correspondence
        };
        
        var address2 = new Address
        {
            City = "Kraków",
            Country = "Polska",
            PostalCode = "31-150",
            Street = "ul. Św. Filipa 17",
            Type = AddressType.Correspondence
        };

        organization.Address = address1;
        person.Address = address2;

        await _dbContext.Organizations.AddAsync(organization);
        await _dbContext.People.AddAsync(person);

        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Zainicjowano przykładowe dane kontaktów w ContactsDbSeeder.");
    }
}
