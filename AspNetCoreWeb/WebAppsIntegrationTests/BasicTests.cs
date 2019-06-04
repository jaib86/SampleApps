using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace WebAppsIntegrationTests
{
    public class BasicTests : IClassFixture<WebApplicationFactory<RazorPagesProject.Startup>>
    {
        private readonly HttpClient client;

        public BasicTests(WebApplicationFactory<RazorPagesProject.Startup> factory)
        {
            this.client = factory.CreateClient();
        }

        [Fact]
        public async Task GetHomePage()
        {
            // Act
            var response = await this.client.GetAsync("/");

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }
    }
}