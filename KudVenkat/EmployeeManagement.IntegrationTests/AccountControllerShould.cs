using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;
using Xunit;

namespace EmployeeManagement.IntegrationTests
{
    public class AccountControllerShould : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture fixture;

        public AccountControllerShould(TestServerFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task<Dictionary<string, string>> LoginApplication()
        {
            // Get initial response that contains anti forgery tokens
            HttpResponseMessage initialResponse = await this.fixture.HttpClient.GetAsync("/Account/Login");
            initialResponse.EnsureSuccessStatusCode();
            var (fieldValue, cookieValue) = await this.fixture.ExtractAntiForgeryValues(initialResponse);

            // Post request
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Account/Login");
            postRequest.Headers.Add("Cookie", new CookieHeaderValue(TestServerFixture.AntiForgeryCookieName, cookieValue).ToString());
            var formData = new Dictionary<string, string>
            {
                { TestServerFixture.AntiForgeryFieldName, fieldValue },
                { "Email", "jack@techjp.in" },
                { "Password", "Jack@1234" },
                { "RememberMe", "False" }
            };
            postRequest.Content = new FormUrlEncodedContent(formData);

            HttpResponseMessage postResponse = await this.fixture.HttpClient.SendAsync(postRequest);
            Assert.Equal(System.Net.HttpStatusCode.Found, postResponse.StatusCode);
            Assert.NotNull(postResponse.Headers);
            Assert.Contains("Set-Cookie: ", postResponse.Headers.ToString());
            Assert.Contains("Location: ", postResponse.Headers.ToString());

            var cookies = postResponse.Headers.GetValues("Set-Cookie");
            var loginCookies = new Dictionary<string, string>();
            foreach (var cookie in cookies)
            {
                string[] cookieParts;

                if (cookie.Contains(';'))
                {
                    cookieParts = cookie.Substring(0, cookie.IndexOf(';')).Split('=');
                }
                else
                {
                    cookieParts = cookie.Split('=');
                }

                if (cookieParts.Length == 2)
                {
                    loginCookies.Add(cookieParts[0], cookieParts[1]);
                }
                else
                {
                    throw new Exception($"Cookie value is not as per the expectation: {cookie}");
                }
            }

            return loginCookies;
        }

        
    }
}