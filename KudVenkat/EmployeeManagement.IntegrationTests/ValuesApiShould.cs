using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace EmployeeManagement.IntegrationTests
{
    public class ValuesApiShould : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture fixture;

        public ValuesApiShould(TestServerFixture fixture)
        {
            this.fixture = fixture;
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(101)]
        [InlineData(102)]
        public async Task GetValidValue(int id)
        {
            var response = await this.fixture.HttpClient.GetAsync($"/api/values/{id}");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Equal($"value {id}", responseString);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-2)]
        [InlineData(-101)]
        [InlineData(-102)]
        public async Task ErrorOnInvalidValue(int id)
        {
            var response = await this.fixture.HttpClient.GetAsync($"/api/values/{id}");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task StartJob()
        {
            var response = await this.fixture.HttpClient.PostAsync("/api/values/startjob", null);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Equal("Batch Job Started", responseString);
        }
    }
}