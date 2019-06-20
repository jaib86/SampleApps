using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Xunit.Abstractions;

namespace WebAppsIntegrationTests
{
    public class BasicTests : IClassFixture<WebApplicationFactory<RazorPagesProject.Startup>>
    {
        private readonly HttpClient client;
        private readonly ITestOutputHelper output;

        public BasicTests(WebApplicationFactory<RazorPagesProject.Startup> factory, ITestOutputHelper output)
        {
            this.client = factory.CreateClient();
            this.output = output;
        }

        [Fact]
        public async Task GetHomePage()
        {
            // Act
            var response = await this.client.GetAsync("/");

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299

            this.output.WriteLine($"{DateTime.Now}: {nameof(HttpResponseMessage.StatusCode)}: {response.StatusCode}");

            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }
    }
}