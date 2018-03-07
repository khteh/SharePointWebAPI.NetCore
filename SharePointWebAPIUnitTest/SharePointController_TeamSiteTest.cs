using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WebAPI.NetCore;
using WebAPI.NetCore.Controllers;
using WebAPI.NetCore.Models;
namespace WebAPIUnitTest
{
    [TestClass]
    public class SharePointController_TeamSiteTest
    {
        private static TestContext testContext_;
        private static string teamCollectionURL_, teamSite_, teamSiteURL_, template_;
        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            testContext_ = context;
            teamCollectionURL_ = "https://dddevops.sharepoint.com";
            teamSite_ = "TestTeam1";
            teamSiteURL_ = $"{teamCollectionURL_}/{teamSite_}";
            template_ = "STS#0";
        }
        [TestMethod]
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
            Assert.IsNotNull(result);
            ObjectResult ok = result as ObjectResult;
            Assert.IsNotNull(ok);
            Assert.AreEqual(200, ok.StatusCode);
            List<SharePointParam> items = ok.Value as List<SharePointParam>;
            Assert.IsNotNull(items);
        }
        [TestMethod]
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
            Assert.IsNotNull(result);
            ObjectResult ok = result as ObjectResult;
            Assert.IsNotNull(ok);
            Assert.AreEqual(200, ok.StatusCode);
            string title = ok.Value as string;
            Assert.IsNotNull(title);
            Assert.AreEqual(title, item.Title);
        }
        [TestMethod]
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
            Assert.IsNotNull(result);
            NoContentResult ok = result as NoContentResult;
            Assert.IsNotNull(ok);
            Assert.AreEqual(StatusCodes.Status204NoContent, ok.StatusCode);
        }
        [TestMethod]
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
            Assert.IsNotNull(result);
            ObjectResult ok = result as ObjectResult;
            Assert.IsNotNull(ok);
            Assert.AreEqual(200, ok.StatusCode);
            List<SharePointTemplate> items = ok.Value as List<SharePointTemplate>;
            Assert.IsNotNull(items);
            Assert.IsTrue(items.Count > 0);
        }
    }
}