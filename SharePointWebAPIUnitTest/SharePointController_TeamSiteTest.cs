using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SharePointWebAPI.NetCore;
using SharePointWebAPI.NetCore.Controllers;
using SharePointWebAPI.NetCore.Models;
using System.Net.Http;
using Xunit;
namespace SharePointWebAPIUnitTest
{
    public class SharePointController_TeamSiteTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private static string teamCollectionURL_, teamSite_, teamSiteURL_, template_;
        public SharePointController_TeamSiteTest(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
            teamCollectionURL_ = "https://dddevops.sharepoint.com";
            teamSite_ = "TestTeam1";
            teamSiteURL_ = $"{teamCollectionURL_}/{teamSite_}";
            template_ = "STS#0";
        }
        [Fact]
        public async Task TeamSitesTest()
        {
            DbContextOptionsBuilder<SharePointContext> optionsBuilder = new DbContextOptionsBuilder<SharePointContext>();
            SharePointContext context = new SharePointContext(optionsBuilder.Options);
            var builder = new ConfigurationBuilder();
            builder.AddUserSecrets<Startup>();
            IConfiguration config = builder.Build();
            SharePointController controller = new SharePointController(config, context);
            // public async Task<List<SharePointItem>> TeamSites(string url)
            IActionResult result = await controller.Sites(teamCollectionURL_);
            Assert.NotNull(result);
            ObjectResult ok = result as ObjectResult;
            Assert.NotNull(ok);
            Assert.Equal(200, ok.StatusCode);
            List<SharePointParam> items = ok.Value as List<SharePointParam>;
            Assert.NotNull(items);
        }
        [Fact]
        public async Task NewTeamSiteTest()
        {
            DbContextOptionsBuilder<SharePointContext> optionsBuilder = new DbContextOptionsBuilder<SharePointContext>();
            SharePointContext context = new SharePointContext(optionsBuilder.Options);
            var builder = new ConfigurationBuilder();
            builder.AddUserSecrets<Startup>();
            IConfiguration config = builder.Build();
            SharePointController controller = new SharePointController(config, context);
            SharePointParam item = new SharePointParam()
            {
                SiteCollectionURL = teamCollectionURL_,
                URL = teamSite_,
                Title = "Test Team 1",
                Description = "Test Team 1 Description",
                Template = template_
            };
            // public async Task<List<SharePointItem>> TeamSites(string url)
            IActionResult result = await controller.NewSite(item);
            Assert.NotNull(result);
            StatusCodeResult ok = result as StatusCodeResult;
            Assert.NotNull(ok);
            Assert.Equal(StatusCodes.Status201Created, ok.StatusCode);
        }
        [Fact]
        public async Task DeleteTeamSiteTest()
        {
            DbContextOptionsBuilder<SharePointContext> optionsBuilder = new DbContextOptionsBuilder<SharePointContext>();
            SharePointContext context = new SharePointContext(optionsBuilder.Options);
            var builder = new ConfigurationBuilder();
            builder.AddUserSecrets<Startup>();
            IConfiguration config = builder.Build();
            SharePointController controller = new SharePointController(config, context);
            // public async Task<List<SharePointItem>> TeamSites(string url)
            IActionResult result = await controller.DeleteSite(teamSiteURL_);
            Assert.NotNull(result);
            NoContentResult ok = result as NoContentResult;
            Assert.NotNull(ok);
            Assert.Equal(StatusCodes.Status204NoContent, ok.StatusCode);
        }
        [Fact]
        public async Task ListTeamTemplatesTest()
        {
            DbContextOptionsBuilder<SharePointContext> optionsBuilder = new DbContextOptionsBuilder<SharePointContext>();
            SharePointContext context = new SharePointContext(optionsBuilder.Options);
            var builder = new ConfigurationBuilder();
            builder.AddUserSecrets<Startup>();
            IConfiguration config = builder.Build();
            SharePointController controller = new SharePointController(config, context);
            // public async Task<List<SharePointItem>> PersonalSites(string url)
            IActionResult result = await controller.Templates(teamCollectionURL_);
            Assert.NotNull(result);
            ObjectResult ok = result as ObjectResult;
            Assert.NotNull(ok);
            Assert.Equal(200, ok.StatusCode);
            List<SharePointTemplate> items = ok.Value as List<SharePointTemplate>;
            Assert.NotNull(items);
            Assert.True(items.Count > 0);
        }
    }
}