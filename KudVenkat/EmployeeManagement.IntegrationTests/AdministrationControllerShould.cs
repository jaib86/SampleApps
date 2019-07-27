using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace EmployeeManagement.IntegrationTests
{
    public class AdministrationControllerShould : BaseControllerShould
    {
        [Fact]
        public async Task ReturnLoginUrlWhenNotLoggedInForCreateRole()
        {
            var response = await this.httpClient.GetAsync("/Administration/CreateRole");

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
            await this.SetLoginCookies(postRequest);

            // Post Response
            HttpResponseMessage postResponse = await this.httpClient.SendAsync(postRequest);

            Assert.Equal(System.Net.HttpStatusCode.Found, postResponse.StatusCode);
            Assert.NotNull(postResponse.Headers);
            Assert.Contains("Location: ", postResponse.Headers.ToString());
            var listRolesLocation = postResponse.Headers.GetValues("Location").First();
            Assert.EndsWith("/Administration/ListRoles", listRolesLocation);

            // Issue New Get with login cookie to check newly created role
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, listRolesLocation);
            await this.SetLoginCookies(httpRequest);
            var response = await this.httpClient.SendAsync(httpRequest);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains($"Role_{ticks}", responseString);
        }
    }
}