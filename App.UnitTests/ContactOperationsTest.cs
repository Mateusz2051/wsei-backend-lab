using System;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Dto;
using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Infrastructure.EF.Context;
using Infrastructure.EF.Repositories;
using Infrastructure.EF.UnitOfWork;
using Infrastructure.Memory;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace App.UnitTests;

public class ContactOperationsTest : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly ContactsDbContext _context;
    private readonly IContactUnitOfWork _uow;
    private readonly IPersonService _personService;
    private readonly IOrganizationService _organizationService;

    public ContactOperationsTest()
    {
        // Set up in-memory SQLite database
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<ContactsDbContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new ContactsDbContext(options);
        _context.Database.EnsureCreated();

        // Initialize repositories
        var personRepo = new EfPersonRepository(_context);
        var companyRepo = new EfCompanyRepository(_context);
        var organizationRepo = new EfOrganizationRepository(_context);

        // Initialize services
        _uow = new EfContactsUnitOfWork(personRepo, companyRepo, organizationRepo, _context);
        _personService = new MemoryPersonService(_uow);
        _organizationService = new OrganizationService(_uow);
    }

    public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }

    [Fact]
    public async Task Organization_Crud_Should_Create_Get_And_Delete_Cleanly()
    {
        // 1. Create Organization
        var address = new AddressDto("Kawiarniana 12", "Krakow", "31-000", "Poland", AddressType.Correspondence);
        var createDto = new CreateOrganizationDto(
            Name: "Academy of Science",
            Type: OrganizationType.Association,
            Krs: "1234567890",
            Website: "https://academy.edu.pl",
            Mission: "Promoting knowledge",
            Email: "office@academy.edu.pl",
            Phone: "123456789",
            Address: address
        );

        var created = await _organizationService.AddOrganization(createDto);
        Assert.NotEqual(Guid.Empty, created.Id);
        Assert.Equal("Academy of Science", created.Name);

        // 2. Read Organization
        var fetched = await _organizationService.GetById(created.Id);
        Assert.NotNull(fetched);
        Assert.Equal("Academy of Science", fetched.Name);
        Assert.Equal(OrganizationType.Association, fetched.Type);

        // 3. Delete Organization
        await _organizationService.DeleteOrganizationAsync(created.Id);
        var deleted = await _organizationService.GetById(created.Id);
        Assert.Null(deleted);
    }

    [Fact]
    public async Task Organization_Members_Should_Link_And_Unlink_Successfully()
    {
        // 1. Arrange: Create Organization and a Person
        var address = new AddressDto("Podchorazych 2", "Krakow", "30-084", "Poland", AddressType.Correspondence);
        var createOrg = new CreateOrganizationDto(
            Name: "Tech Hub",
            Type: OrganizationType.Foundation,
            Krs: "0987654321",
            Website: "https://techhub.org",
            Mission: "Supporting tech",
            Email: "contact@techhub.org",
            Phone: "987654321",
            Address: address
        );
        var organization = await _organizationService.AddOrganization(createOrg);

        var createPerson = new CreatePersonDto(
            FirstName: "Jan",
            LastName: "Kowalski",
            Email: "jan.kowalski@techhub.org",
            Phone: "500600700",
            Position: "Developer",
            BirthDate: DateTime.Parse("1995-05-15"),
            Gender: Gender.Male,
            EmployerId: null,
            Address: address,
            Pesel: "02252012343"
        );
        var person = await _personService.AddPerson(createPerson);

        // 2. Act: Link member
        await _organizationService.AddMemberAsync(organization.Id, person.Id);

        // 3. Assert: Verify member is linked
        var members = (await _organizationService.GetMembersAsync(organization.Id)).ToList();
        Assert.Single(members);
        Assert.Equal(person.Id, members[0].Id);
        Assert.Equal("Jan", members[0].FirstName);
        Assert.Equal("Kowalski", members[0].LastName);

        var personDto = await _personService.GetById(person.Id);
        Assert.NotNull(personDto);
        // Note: Check if the organization field or search by organization ID returns the person
        var searchResult = await _personService.SearchPeopleAsync(null, organization.Id, null);
        Assert.Single(searchResult);
        Assert.Equal(person.Id, searchResult.First().Id);

        // 4. Act: Unlink member
        await _organizationService.RemoveMemberAsync(organization.Id, person.Id);

        // 5. Assert: Verify member is unlinked
        var membersAfterRemoval = await _organizationService.GetMembersAsync(organization.Id);
        Assert.Empty(membersAfterRemoval);

        var searchAfterRemoval = await _personService.SearchPeopleAsync(null, organization.Id, null);
        Assert.Empty(searchAfterRemoval);
    }

    [Fact]
    public async Task Delete_Organization_Should_Disassociate_Members_Cleanly()
    {
        // 1. Arrange: Create Organization and a Person, then link them
        var address = new AddressDto("Rynek 1", "Krakow", "31-000", "Poland", AddressType.Correspondence);
        var organization = await _organizationService.AddOrganization(new CreateOrganizationDto(
            Name: "Club X",
            Type: OrganizationType.Association,
            Krs: null,
            Website: null,
            Mission: null,
            Email: "clubx@example.com",
            Phone: "111222333",
            Address: address
        ));

        var person = await _personService.AddPerson(new CreatePersonDto(
            FirstName: "Maria",
            LastName: "Nowak",
            Email: "maria@example.com",
            Phone: "222333444",
            Position: "Manager",
            BirthDate: DateTime.Parse("1990-10-10"),
            Gender: Gender.Female,
            EmployerId: null,
            Address: address
        ));

        await _organizationService.AddMemberAsync(organization.Id, person.Id);

        // Verify member is linked
        var members = await _organizationService.GetMembersAsync(organization.Id);
        Assert.Single(members);

        // 2. Act: Delete Organization
        await _organizationService.DeleteOrganizationAsync(organization.Id);

        // 3. Assert: Person should still exist, but Organization link should be null
        var personAfter = await _personService.GetById(person.Id);
        Assert.NotNull(personAfter);
        
        var searchByOrg = await _personService.SearchPeopleAsync(null, organization.Id, null);
        Assert.Empty(searchByOrg);
    }

    [Fact]
    public async Task Advanced_Contact_Search_Should_Filter_By_Proposed_Criteria()
    {
        var address = new AddressDto("Pilotow 1", "Krakow", "31-000", "Poland", AddressType.Correspondence);

        // 1. Arrange: Create Companies (Employer)
        var company = new Company
        {
            Id = Guid.NewGuid(),
            Name = "Aviation Corp",
            Industry = "Aerospace",
            Email = "office@aviation.com",
            Phone = "123456",
            Address = new Address
            {
                Street = address.Street,
                City = address.City,
                PostalCode = address.PostalCode,
                Country = address.Country,
                Type = address.Type
            }
        };
        _context.Companies.Add(company);
        await _context.SaveChangesAsync();

        // 2. Arrange: Create Organizations
        var org = await _organizationService.AddOrganization(new CreateOrganizationDto(
            Name: "Science Club",
            Type: OrganizationType.Association,
            Krs: null,
            Website: null,
            Mission: null,
            Email: "club@science.org",
            Phone: "654321",
            Address: address
        ));

        // Create 3 people with different parameters:
        // Person 1: Email = techteam.com, Org = Science Club, Company = null
        var p1 = await _personService.AddPerson(new CreatePersonDto(
            FirstName: "Andrzej",
            LastName: "Duda",
            Email: "andrzej@techteam.com",
            Phone: "501202303",
            Position: null,
            BirthDate: DateTime.Parse("1980-01-01"),
            Gender: Gender.Male,
            EmployerId: null,
            Address: address
        ));
        await _organizationService.AddMemberAsync(org.Id, p1.Id);

        // Person 2: Email = techteam.com, Org = null, Company = Aviation Corp
        var p2 = await _personService.AddPerson(new CreatePersonDto(
            FirstName: "Beata",
            LastName: "Szydlo",
            Email: "beata@techteam.com",
            Phone: "502303404",
            Position: null,
            BirthDate: DateTime.Parse("1985-02-02"),
            Gender: Gender.Female,
            EmployerId: company.Id,
            Address: address
        ));

        // Person 3: Email = gmail.com, Org = null, Company = Aviation Corp
        var p3 = await _personService.AddPerson(new CreatePersonDto(
            FirstName: "Donald",
            LastName: "Tusk",
            Email: "donald@gmail.com",
            Phone: "503404505",
            Position: null,
            BirthDate: DateTime.Parse("1975-03-03"),
            Gender: Gender.Male,
            EmployerId: company.Id,
            Address: address
        ));

        // 3. Act & Assert: Search 1 - by email domain "@techteam.com"
        var searchEmail1 = (await _personService.SearchPeopleAsync("techteam.com", null, null)).ToList();
        Assert.Equal(2, searchEmail1.Count);
        Assert.Contains(searchEmail1, x => x.Id == p1.Id);
        Assert.Contains(searchEmail1, x => x.Id == p2.Id);

        // Search 2 - by email domain "@gmail.com"
        var searchEmail2 = (await _personService.SearchPeopleAsync("@gmail.com", null, null)).ToList();
        Assert.Single(searchEmail2);
        Assert.Equal(p3.Id, searchEmail2[0].Id);

        // Search 3 - by organization id
        var searchOrg = (await _personService.SearchPeopleAsync(null, org.Id, null)).ToList();
        Assert.Single(searchOrg);
        Assert.Equal(p1.Id, searchOrg[0].Id);

        // Search 4 - by company id
        var searchCompany = (await _personService.SearchPeopleAsync(null, null, company.Id)).ToList();
        Assert.Equal(2, searchCompany.Count);
        Assert.Contains(searchCompany, x => x.Id == p2.Id);
        Assert.Contains(searchCompany, x => x.Id == p3.Id);

        // Search 5 - combined (email domain + company id)
        var searchCombined = (await _personService.SearchPeopleAsync("techteam.com", null, company.Id)).ToList();
        Assert.Single(searchCombined);
        Assert.Equal(p2.Id, searchCombined[0].Id);
    }

    [Fact]
    public async Task Pesel_Persistence_Should_Map_ValueObject_Correctly_In_Database()
    {
        // 1. Arrange: Create person with valid PESEL
        var address = new AddressDto("Urzad 1", "Warszawa", "00-001", "Poland", AddressType.Correspondence);
        var createDto = new CreatePersonDto(
            FirstName: "Krzysztof",
            LastName: "Zanussi",
            Email: "krzysztof@example.com",
            Phone: "505606707",
            Position: "Director",
            BirthDate: DateTime.Parse("1970-07-07"),
            Gender: Gender.Male,
            EmployerId: null,
            Address: address,
            Pesel: "02252012350"
        );

        var person = await _personService.AddPerson(createDto);
        Assert.NotNull(person.Pesel);
        Assert.Equal("02252012350", person.Pesel.Value);

        // 2. Act: Fetch directly from DbContext to verify Value Converter persisted it as string
        var rawPerson = await _context.People.AsNoTracking().FirstOrDefaultAsync(p => p.Id == person.Id);
        Assert.NotNull(rawPerson);
        Assert.NotNull(rawPerson.Pesel);
        Assert.Equal("02252012350", rawPerson.Pesel.Value);

        // Verify with Service Layer
        var fetchedDto = await _personService.GetById(person.Id);
        Assert.NotNull(fetchedDto);
        Assert.Equal("02252012350", fetchedDto.Pesel);

        // 3. Act: Update PESEL to a different valid PESEL
        var updateDto = new UpdatePersonDto(
            FirstName: null,
            LastName: null,
            Email: null,
            Phone: null,
            Position: null,
            BirthDate: null,
            Gender: null,
            EmployerId: null,
            Address: null,
            Status: null,
            Pesel: "02252012367"
        );

        var updated = await _personService.UpdatePerson(person.Id, updateDto);
        Assert.NotNull(updated.Pesel);
        Assert.Equal("02252012367", updated.Pesel.Value);

        // Verify retrieval of updated PESEL
        var fetchedDto2 = await _personService.GetById(person.Id);
        Assert.NotNull(fetchedDto2);
        Assert.Equal("02252012367", fetchedDto2.Pesel);
    }
}
