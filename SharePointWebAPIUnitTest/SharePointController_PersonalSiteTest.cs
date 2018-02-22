using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WebAPI.NetCore;
using WebAPI.NetCore.Controllers;
using WebAPI.NetCore.Models;
namespace WebAPIUnitTest
{
    [TestClass]
    public class SharePointController_PersonalSiteTest
    {
        private static TestContext testContext_;
        private static string personalCollectionURL_, personalSite_, personalSiteURL_, template_;
        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            testContext_ = context;
            personalCollectionURL_ = "https://dddevops-my.sharepoint.com";
            personalSite_ = "TestPersonal1";
            personalSiteURL_ = $"{personalCollectionURL_}/{personalSite_}";
            template_ = "STS#0";
        }
        [TestMethod]
        public async Task PersonalSitesTest()
        {
            DbContextOptionsBuilder<SharePointContext> optionsBuilder = new DbContextOptionsBuilder<SharePointContext>();
            SharePointContext context = new SharePointContext(optionsBuilder.Options);
            var builder = new ConfigurationBuilder();
            builder.AddUserSecrets<Startup>();
            IConfiguration config = builder.Build();
            SharePointController controller = new SharePointController(config, context);
            // public async Task<List<SharePointItem>> PersonalSites(string url)
            List<SharePointItem> items = await controller.Sites(personalCollectionURL_);
            Assert.IsNotNull(items);
        }
        [TestMethod]
        public async Task NewPersonalSiteTest()
        {
            DbContextOptionsBuilder<SharePointContext> optionsBuilder = new DbContextOptionsBuilder<SharePointContext>();
            SharePointContext context = new SharePointContext(optionsBuilder.Options);
            var builder = new ConfigurationBuilder();
            builder.AddUserSecrets<Startup>();
            IConfiguration config = builder.Build();
            SharePointController controller = new SharePointController(config, context);
            SharePointItem item = new SharePointItem()
            {
                SiteCollectionURL = personalCollectionURL_,
                URL = personalSite_,
                Title = "Test Personal 1",
                Template = template_
            };
            // public async Task<List<SharePointItem>> PersonalSites(string url)
            IActionResult result = await controller.NewSite(item);
            Assert.IsNotNull(result);
            ObjectResult ok = result as ObjectResult;
            Assert.IsNotNull(ok);
            string title = ok.Value as string;
            Assert.IsNotNull(title);
            Assert.AreEqual(title, item.Title);
        }
        [TestMethod]
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
            Assert.IsNotNull(result);
            NoContentResult ok = result as NoContentResult;
            Assert.IsNotNull(ok);
            Assert.AreEqual(StatusCodes.Status204NoContent, ok.StatusCode);
        }
        [TestMethod]
        public async Task ListPersonalTemplatesTest()
        {
            DbContextOptionsBuilder<SharePointContext> optionsBuilder = new DbContextOptionsBuilder<SharePointContext>();
            SharePointContext context = new SharePointContext(optionsBuilder.Options);
            var builder = new ConfigurationBuilder();
            builder.AddUserSecrets<Startup>();
            IConfiguration config = builder.Build();
            SharePointController controller = new SharePointController(config, context);
            // public async Task<List<SharePointItem>> PersonalSites(string url)
            List<SharePointTemplate> items = await controller.Templates(personalCollectionURL_);
            Assert.IsNotNull(items);
            Assert.IsTrue(items.Count > 0);
        }
    }
}