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

namespace EmployeeManagement.IntegrationTests
{
    public class TestServerFixture : IDisposable
    {
        // Flag: Has Dispose already been called?
        private bool disposed;

        private readonly TestServer testServer;

        public const string AntiForgeryFieldName = "__AFTField";
        public const string AntiForgeryCookieName = "AFTCookie";

        public HttpClient HttpClient { get; }

        public TestServerFixture()
        {
            var builder = new WebHostBuilder()
                          .UseContentRoot(@"D:\practices\code\SampleApps\KudVenkat\EmployeeManagement")
                          .UseEnvironment("Testing")
                          .UseSetting("TestEmpDBConn", "server=JAIPRAKASH31496\\SQLEXPRESS;database=EmployeeDb;Trusted_Connection=true")
                          .UseStartup<Startup>()
                          .ConfigureServices(x => x.AddAntiforgery(t =>
                           {
                               t.FormFieldName = AntiForgeryFieldName;
                               t.Cookie.Name = AntiForgeryCookieName;
                           }));

            this.testServer = new TestServer(builder);

            this.HttpClient = this.testServer.CreateClient();
        }

        public async Task<(string fieldValue, string cookieValue)> ExtractAntiForgeryValues(HttpResponseMessage responseMessage)
        {
            return (this.ExtractAntiforgeryToken(await responseMessage.Content.ReadAsStringAsync()),
                    this.ExtractAntiforgeryCookieValue(responseMessage));
        }

        public void SetLoginCookies(HttpRequestMessage httpRequest, Dictionary<string, string> loginCookies)
        {
            if (httpRequest != null && loginCookies?.Count > 0)
            {
                // .AspNetCore.Identity.Application
                httpRequest.Headers.Add("Cookie", loginCookies.Select(c => new CookieHeaderValue(c.Key, c.Value).ToString()));
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

        public void Dispose()
        {
            // Dispose of unmanaged resources
            this.Dispose(true);
            // Suppress finalization
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                // Free managed objects here.
            }

            // Free unmanaged objects here.
            this.HttpClient.Dispose();
            this.testServer.Dispose();

            this.disposed = true;
        }

        ~TestServerFixture()
        {
            this.Dispose(false);
        }
    }
}