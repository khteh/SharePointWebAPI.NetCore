using System.Threading.Tasks;
using SharePointWebAPI.NetCore;
using Xunit;
using System.Net.Http;
using Newtonsoft.Json;
using SharePointWebAPI.NetCore.Models.Request;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Net;

namespace SharePointWebAPIUnitTest
{
    public class TokenControllerTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        public TokenControllerTest(CustomWebApplicationFactory<Startup> factory) => _client = factory.CreateClient();
        [Fact]
        public async Task CreateTokenTest()
        {
            var httpResponse = await _client.PostAsync("/api/token/create", new StringContent(JsonConvert.SerializeObject(new TokenRequest("khteh@dddevops.onmicrosoft.com", "Pa$$w0rd")), Encoding.UTF8, "application/json"));
            httpResponse.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
            var token = await httpResponse.Content.ReadAsStringAsync();
            Assert.False(string.IsNullOrEmpty(token));
            string[] parts = token.Split('.');
            Assert.Equal(3, parts.Length);
            foreach (string part in parts)
                Assert.False(string.IsNullOrEmpty(part));
        }
    }
}