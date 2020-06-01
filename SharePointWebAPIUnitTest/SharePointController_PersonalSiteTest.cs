using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SharePointWebAPI.NetCore;
using SharePointWebAPI.NetCore.Controllers;
using SharePointWebAPI.NetCore.Models;
using Xunit;
using System.Net.Http;

namespace SharePointWebAPIUnitTest
{
    public class SharePointController_PersonalSiteTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private static string personalCollectionURL_, personalSite_, personalSiteURL_, template_;
        public SharePointController_PersonalSiteTest(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
            personalCollectionURL_ = "https://dddevops-my.sharepoint.com";
            personalSite_ = "TestPersonal1";
            personalSiteURL_ = $"{personalCollectionURL_}/{personalSite_}";
            template_ = "STS#0";
        }
        [Fact]
        public async Task PersonalSitesTest()
        {
            DbContextOptionsBuilder<SharePointContext> optionsBuilder = new DbContextOptionsBuilder<SharePointContext>();
            SharePointContext context = new SharePointContext(optionsBuilder.Options);
            var builder = new ConfigurationBuilder();
            builder.AddUserSecrets<Startup>();
            IConfiguration config = builder.Build();
            SharePointController controller = new SharePointController(config, context);
            // public async Task<List<SharePointItem>> PersonalSites(string url)
            IActionResult result = await controller.Sites(personalCollectionURL_);
            Assert.NotNull(result);
            ObjectResult ok = result as ObjectResult;
            Assert.NotNull(ok);
            Assert.Equal(200, ok.StatusCode);
            List<SharePointParam> items = ok.Value as List<SharePointParam>;
            Assert.NotNull(items);
        }
        [Fact]
        public async Task NewPersonalSiteTest()
        {
            DbContextOptionsBuilder<SharePointContext> optionsBuilder = new DbContextOptionsBuilder<SharePointContext>();
            SharePointContext context = new SharePointContext(optionsBuilder.Options);
            var builder = new ConfigurationBuilder();
            builder.AddUserSecrets<Startup>();
            IConfiguration config = builder.Build();
            SharePointController controller = new SharePointController(config, context);
            SharePointParam item = new SharePointParam()
            {
                SiteCollectionURL = personalCollectionURL_,
                URL = personalSite_,
                Title = "Test Personal 1",
                Description = "Test Personal Site Description",
                Template = template_
            };
            // public async Task<List<SharePointItem>> PersonalSites(string url)
            IActionResult result = await controller.NewSite(item);
            Assert.NotNull(result);
            StatusCodeResult ok = result as StatusCodeResult;
            Assert.NotNull(ok);
            Assert.Equal(StatusCodes.Status201Created, ok.StatusCode);
        }
        [Fact]
        public async Task DeletePersonalSiteTest()
        {
            DbContextOptionsBuilder<SharePointContext> optionsBuilder = new DbContextOptionsBuilder<SharePointContext>();
            SharePointContext context = new SharePointContext(optionsBuilder.Options);
            var builder = new ConfigurationBuilder();
            builder.AddUserSecrets<Startup>();
            IConfiguration config = builder.Build();
            SharePointController controller = new SharePointController(config, context);
            // public async Task<List<SharePointItem>> PersonalSites(string url)
            IActionResult result = await controller.DeleteSite(personalSiteURL_);
            Assert.NotNull(result);
            NoContentResult ok = result as NoContentResult;
            Assert.NotNull(ok);
            Assert.Equal(StatusCodes.Status204NoContent, ok.StatusCode);
        }
        [Fact]
        public async Task ListPersonalTemplatesTest()
        {
            DbContextOptionsBuilder<SharePointContext> optionsBuilder = new DbContextOptionsBuilder<SharePointContext>();
            SharePointContext context = new SharePointContext(optionsBuilder.Options);
            var builder = new ConfigurationBuilder();
            builder.AddUserSecrets<Startup>();
            IConfiguration config = builder.Build();
            SharePointController controller = new SharePointController(config, context);
            // public async Task<List<SharePointItem>> PersonalSites(string url)
            IActionResult result = await controller.Templates(personalCollectionURL_);
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