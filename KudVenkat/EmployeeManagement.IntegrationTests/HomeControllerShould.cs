using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace EmployeeManagement.IntegrationTests
{
    public class HomeControllerShould
    {
        [Fact]
        public async Task RenderApplicationForm()
        {
            var builder = new WebHostBuilder().UseContentRoot(@"D:\practices\code\SampleApps\KudVenkat\EmployeeManagement")
                                              .UseEnvironment("Testing")
                                              .UseStartup<Startup>();

            var server = new TestServer(builder);

            var client = server.CreateClient();

            var response = await client.GetAsync("/");

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            Assert.Contains("value1", responseString);
        }
    }
}