using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace App.IntegrationTests;

public class ContactsEndpointTests : IClassFixture<WebApplicationFactory<BackendLab01.Program>>
{
    private readonly WebApplicationFactory<BackendLab01.Program> _factory;

    public ContactsEndpointTests(WebApplicationFactory<BackendLab01.Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetAllPersons_WithoutAuth_ReturnsUnauthorized()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/contacts");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
