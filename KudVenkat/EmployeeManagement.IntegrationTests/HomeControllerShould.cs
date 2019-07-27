using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace EmployeeManagement.IntegrationTests
{
    public class HomeControllerShould : BaseControllerShould
    {
        [Fact]
        public async Task RenderDefaultPage()
        {
            var response = await this.httpClient.GetAsync("/");

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains("<title>Employee List</title>", responseString);
        }

        [Fact]
        public async Task RenderDefaultHomePage()
        {
            var response = await this.httpClient.GetAsync("/Home");

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains("<title>Employee List</title>", responseString);
        }

        [Fact]
        public async Task RenderHomeIndexPage()
        {
            var response = await this.httpClient.GetAsync("/Home/Index");

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains("<title>Employee List</title>", responseString);
        }

        
        [Fact]
        public async Task CreateNewEmployee()
        {
            var ticks = System.DateTime.Now.Ticks;
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Home/Create");
            var formData = new Dictionary<string, string>
            {
                { "Name", $"Jack_{ticks}" },
                { "Email", $"Jack_{ticks}@techjp.in" },
                { "Department", "1" }
            };
            postRequest.Content = new FormUrlEncodedContent(formData);
            await this.SetLoginCookies(postRequest);

            // Post Response
            HttpResponseMessage postResponse = await this.httpClient.SendAsync(postRequest);

            Assert.Equal(System.Net.HttpStatusCode.Found, postResponse.StatusCode);
            Assert.NotNull(postResponse.Headers);
            Assert.Contains("Location: ", postResponse.Headers.ToString());
            var newEmployeeLocation = postResponse.Headers.GetValues("Location").First();

            // Issue New Get for newly created employee
            var response = await this.httpClient.GetAsync(newEmployeeLocation);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains($"<h1>Jack_{ticks}</h1>", responseString);
            Assert.Contains($"Email: Jack_{ticks}@techjp.in", responseString);
        }
    }
}