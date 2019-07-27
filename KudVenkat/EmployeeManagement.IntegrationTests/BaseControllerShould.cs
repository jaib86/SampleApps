using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Xunit;

namespace EmployeeManagement.IntegrationTests
{
    public class BaseControllerShould
    {
        protected readonly HttpClient httpClient;
        protected Dictionary<string, string> loginCookies;
        private const string AntiForgeryFieldName = "__AFTField";
        private const string AntiForgeryCookieName = "AFTCookie";

        public BaseControllerShould()
        {
            var builder = new WebHostBuilder().UseContentRoot(@"D:\practices\code\SampleApps\KudVenkat\EmployeeManagement")
                                              .UseEnvironment("Testing")
                                              .UseSetting("TestEmpDBConn",
                                              "server=JAIPRAKASH31496\\SQLEXPRESS;database=EmployeeDb;Trusted_Connection=true")
                                              .UseStartup<Startup>()
                                              .ConfigureServices(x => x.AddAntiforgery(t =>
                                               {
                                                   t.FormFieldName = AntiForgeryFieldName;
                                                   t.Cookie.Name = AntiForgeryCookieName;
                                               }));

            var server = new TestServer(builder);

            this.httpClient = server.CreateClient();
        }

        [Fact]
        public async Task LoginApplication()
        {
            // Get initial response that contains anti forgery tokens
            HttpResponseMessage initialResponse = await this.httpClient.GetAsync("/Account/Login");
            initialResponse.EnsureSuccessStatusCode();
            string antiForgeryCookieValue = this.ExtractAntiforgeryCookieValue(initialResponse);
            string antiForgeryToken = this.ExtractAntiforgeryToken(await initialResponse.Content.ReadAsStringAsync());

            // Post request
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Account/Login");
            postRequest.Headers.Add("Cookie", new CookieHeaderValue(AntiForgeryCookieName, antiForgeryCookieValue).ToString());
            var formData = new Dictionary<string, string>
            {
                { AntiForgeryFieldName, antiForgeryToken },
                { "Email", "jack@techjp.in" },
                { "Password", "Jack@1234" },
                { "RememberMe", "False" }
            };
            postRequest.Content = new FormUrlEncodedContent(formData);

            HttpResponseMessage postResponse = await this.httpClient.SendAsync(postRequest);

            Assert.Equal(System.Net.HttpStatusCode.Found, postResponse.StatusCode);
            Assert.NotNull(postResponse.Headers);
            Assert.Contains("Set-Cookie: ", postResponse.Headers.ToString());
            Assert.Contains("Location: ", postResponse.Headers.ToString());

            var cookies = postResponse.Headers.GetValues("Set-Cookie");
            this.loginCookies = new Dictionary<string, string>();
            //{
            //    { ".AspNetCore.Antiforgery.tqjgdde0exU", "CfDJ8OErlx3oTn9Io9_7InaMeyD-YQtJzw5cHI3UfhtLAg5zdQaD3bCEkTKqXMd6axX9dM5xS1GQ5BYPeV0wDTPAvOn0a4bygWC5G7Rf_9YGaayZI6blfQbZIFKsKTQLcOpaQraYWPHgRZZ_po8n7tz5Euk" }
            //};

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
                    this.loginCookies.Add(cookieParts[0], cookieParts[1]);
                }
                else
                {
                    throw new Exception($"Cookie value is not as per the expectation: {cookie}");
                }
            }
        }

        protected async Task SetLoginCookies(HttpRequestMessage httpRequest)
        {
            if (this.loginCookies == null)
            {
                await this.LoginApplication();
            }

            // .AspNetCore.Identity.Application
            httpRequest.Headers.Add("Cookie", this.loginCookies.Select(c => new CookieHeaderValue(c.Key, c.Value).ToString()));
        }

        private string ExtractAntiforgeryCookieValue(HttpResponseMessage responseMessage)
        {
            var antiForgeryCookie = responseMessage.Headers.GetValues("Set-Cookie").FirstOrDefault(x => x.Contains(AntiForgeryCookieName));

            if (antiForgeryCookie is null)
            {
                throw new ArgumentException($"Cookie '{AntiForgeryCookieName}' not found in HTTP response");
            }
            else
            {
                var antiForgeryCookieValue = SetCookieHeaderValue.Parse(antiForgeryCookie).Value;
                return antiForgeryCookieValue.ToString();
            }
        }

        private string ExtractAntiforgeryToken(string htmlBody)
        {
            var requestVerificationTokenMatch = Regex.Match(htmlBody, $@"\<input name=""{AntiForgeryFieldName}"" type=""hidden"" value=""([^""]+)"" \/\>");

            if (requestVerificationTokenMatch.Success)
            {
                return requestVerificationTokenMatch.Groups[1].Captures[0].Value;
            }
            else
            {
                throw new ArgumentException($"Anti forgery token '{AntiForgeryFieldName}' not found in HTML", nameof(htmlBody));
            }
        }
    }
}