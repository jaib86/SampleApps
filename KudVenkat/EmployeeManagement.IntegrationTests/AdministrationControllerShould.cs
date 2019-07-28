using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace EmployeeManagement.IntegrationTests
{
    public class AdministrationControllerShould : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture fixture;

        public AdministrationControllerShould(TestServerFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task ReturnLoginUrlWhenNotLoggedInForCreateRole()
        {
            var response = await this.fixture.HttpClient.GetAsync("/Administration/CreateRole");

            Assert.Equal(System.Net.HttpStatusCode.Found, response.StatusCode);
            Assert.NotNull(response.Headers);
            Assert.Contains("Location: ", response.Headers.ToString());

            var loginWithReturnUrl = response.Headers.GetValues("Location").First();
            Assert.EndsWith("/Account/Login?ReturnUrl=%2FAdministration%2FCreateRole", loginWithReturnUrl);
        }

        [Fact]
        public async Task CreateRole()
        {
            var ticks = System.DateTime.Now.Ticks;
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Administration/CreateRole");
            var formData = new Dictionary<string, string> { { "RoleName", $"Role_{ticks}" } };
            postRequest.Content = new FormUrlEncodedContent(formData);

            // .AspNetCore.Identity.Application
            var accountController = new AccountControllerShould(this.fixture);
            var loginCookies = await accountController.LoginApplication();
            this.fixture.SetLoginCookies(postRequest, loginCookies);

            // Post Response
            HttpResponseMessage postResponse = await this.fixture.HttpClient.SendAsync(postRequest);

            Assert.Equal(System.Net.HttpStatusCode.Found, postResponse.StatusCode);
            Assert.NotNull(postResponse.Headers);
            Assert.Contains("Location: ", postResponse.Headers.ToString());
            var listRolesLocation = postResponse.Headers.GetValues("Location").First();
            Assert.EndsWith("/Administration/ListRoles", listRolesLocation);

            // Issue New Get with login cookie to check newly created role
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, listRolesLocation);
            this.fixture.SetLoginCookies(httpRequest, loginCookies);
            var response = await this.fixture.HttpClient.SendAsync(httpRequest);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains($"Role_{ticks}", responseString);
        }
    }
}