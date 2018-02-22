using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.NetCore.Controllers;
using WebAPI.NetCore.Models;

namespace WebAPIUnitTest
{
    [TestClass]
    public class TokenControllerTest
    {
        private static TestContext testContext_;
        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            testContext_ = context;
        }
        [TestMethod]
        public void CreateTokenTest()
        {
            TokenController controller = new TokenController();
            IActionResult result = controller.Create("khteh@dddevops.onmicrosoft.com", "Pa$$w0rd");
            Assert.IsNotNull(result);
            ObjectResult obj = result as ObjectResult;
            Assert.IsNotNull(obj);
            string token = obj.Value as string;
            Assert.IsNotNull(token);
            Assert.IsFalse(string.IsNullOrEmpty(token));
            string[] parts = token.Split('.');
            Assert.AreEqual(3, parts.Length);
            foreach (string part in parts)
                Assert.IsFalse(string.IsNullOrEmpty(part));
        }
    }
}