using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
//using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.NetCore.Models;

namespace WebAPI.NetCore.Controllers
{
    /// <summary>
    /// SharePoint Web API controller
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class SharePointController : Controller
    {
        private readonly string _username, _password;
        private readonly SharePointContext _context;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="config"></param>
        /// <param name="context"></param>
        public SharePointController(IConfiguration config, SharePointContext context)
        {
            _username = config["Authentication:SharePoint:Username"];
            _password = config["Authentication:SharePoint:Password"];
            _context = context;
        }
        /// <summary>
        /// Delete a site
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /api/sharepoint/deletesite
        ///     {
        ///        "url": "https://....com/testteam1"
        ///     }
        /// </remarks>
        /// <param name="url">The site url to delete</param>
        /// <returns></returns>
        /// <response code="204">Returns success with No-content result</response>
        /// <response code="500">If the input parameter is null or empty</response>
        [HttpDelete("{url}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteSite(string url)
        {
            // Starting with ClientContext, the constructor requires a URL to the 
            // server running SharePoint.
            //string teamURL = @"https://dddevops.sharepoint.com/TeamSite1";
            using (ClientContext context = new ClientContext(url))
            try
            {
                //Web web = context.Web;
                //context.Load(web);
                //context.Credentials = new NetworkCredential("khteh", "", "dddevops.onmicrosoft.com");
                context.Credentials = new SharePointOnlineCredentials(_username, _password);
                Web web = context.Web;
                // Retrieve the new web information. 
                context.Load(web);
                //context.Load(newWeb);
                await context.ExecuteQueryAsync();
                web.DeleteObject();
                await context.ExecuteQueryAsync();
                return new NoContentResult();
            } catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
        /// <summary>
        /// Create a new site
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/sharepoint/newsite
        ///     {
        ///        "param": {
        ///             "SiteCollectionURL": "https://....com",
        ///             "Title": "Test Team 1",
        ///             "URL": "TestTeam1",
        ///             "Description": "Test Team 1 description",
        ///             "Template": "STS#0"
        ///        }
        ///     }
        /// </remarks>
        /// <param name="param">Site creation parameters</param>
        /// <returns></returns>
        /// <response code="200">Returns success with the new site title</response>
        /// <response code="500">If the input parameter is null or empty</response>
        [HttpGet("{param}", Name = "NewSite")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> NewSite(SharePointItem param)
        {
            // Starting with ClientContext, the constructor requires a URL to the 
            // server running SharePoint.
            //string teamURL = @"https://dddevops.sharepoint.com";
            using (ClientContext context = new ClientContext(param.SiteCollectionURL))
            try
            {
                //Web web = context.Web;
                //context.Load(web);
                WebCreationInformation creation = new WebCreationInformation();
                //context.Credentials = new NetworkCredential("khteh", "", "dddevops.onmicrosoft.com");
                context.Credentials = new SharePointOnlineCredentials(_username, _password);
                creation.Url = param.URL;
                creation.Title = param.Title;
                creation.Description = param.Description;
                creation.UseSamePermissionsAsParentSite = true;
                creation.WebTemplate = param.Template;//"STS#0";
                creation.Language = 1033;
                Web newWeb = context.Web.Webs.Add(creation);
                // Retrieve the new web information. 
                context.Load(newWeb, w => w.Title);
                //context.Load(newWeb);
                await context.ExecuteQueryAsync();
                return new OkObjectResult(newWeb.Title);
            } catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
#if false
        This section is commented out due to the fact that current Microsoft SharePoint Cliet component SDK does NOT support tennat administration for .Net Core
        [HttpGet]
        public List<SharePointItem> SiteCollections()
        {
            List<SharePointItem> results = new List<SharePointItem>();
            SPOSitePropertiesEnumerable prop = null;
            string tenantAdminURL = @"https://dddevops-admin.sharepoint.com/";
            using (ClientContext context = new ClientContext(tenantAdminURL))
            {
                SecureString securePassword = new SecureString();
                foreach (char c in password.ToCharArray())
                    securePassword.AppendChar(c);
                context.Credentials = new SharePointOnlineCredentials(username, securePassword);
                Tenant tenant = new Tenant(context);
                prop = tenant.GetSiteProperties(0, true);
                context.Load(prop);
                context.ExecuteQuery();
                foreach (SiteProperties sp in prop)
                {
                    SharePointItem item = new SharePointItem() { Title = sp.Title, URL = sp.Url };
                    results.Add(item);
                }
            }
            return results;
        }
#endif
        /// <summary>
        /// Retrieve all the sites of a specified site collection
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/sharepoint/sites
        ///     {
        ///        "url": "https://....com"
        ///     }
        /// </remarks>
        /// <param name="url">Site collection URL</param>
        /// <returns></returns>
        [HttpGet("{url}", Name = "GetSites")]
        public async Task<IActionResult> Sites(string url)
        {
            List<SharePointItem> results = new List<SharePointItem>();
            try
            {
                // Starting with ClientContext, the constructor requires a URL to the 
                // server running SharePoint.
                //string url = @"https://dddevops.sharepoint.com/";
                using (ClientContext context = new ClientContext(url))
                {
                    context.Credentials = new SharePointOnlineCredentials(_username, _password);
                    // Root Web Site 
                    Web spRootWebSite = context.Web;
                    // Collecction of Sites under the Root Web Site 
                    WebCollection spSites = spRootWebSite.Webs;
                    // Loading operations         
                    context.Load(spRootWebSite);
                    context.Load(spSites);
                    await context.ExecuteQueryAsync();
                    List<Task> tasks = new List<Task>();
                    // We need to iterate through the $spoSites Object in order to get individual sites information 
                    foreach (Web site in spSites)
                    {
                        context.Load(site);
                        tasks.Add(context.ExecuteQueryAsync());
                    }
                    Task.WaitAll(tasks.ToArray());
                    foreach (Web site in spSites)
                    {
                        SharePointItem item = new SharePointItem() { Title = site.Title, URL = site.Url };
                        results.Add(item);
                    }
                }
            } catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
            return new OkObjectResult(results);
        }
        /// <summary>
        /// Retrieve the available templates of a site collection
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/sharepoint/templates
        ///     {
        ///        "url": "https://....com"
        ///     }
        /// </remarks>
        /// <param name="url">URL of a site collection to retrieve the templates from</param>
        /// <returns></returns>
        [HttpGet("{url}", Name = "GetTemplates")]
        public async Task<IActionResult> Templates(string url)
        {
            List<SharePointTemplate> results = new List<SharePointTemplate>();
            try
            {
                // Starting with ClientContext, the constructor requires a URL to the 
                // server running SharePoint.
                //string url = @"https://dddevops-my.sharepoint.com";
                using (ClientContext context = new ClientContext(url))
                {
                    context.Credentials = new SharePointOnlineCredentials(_username, _password);
                    Web web = context.Web;
                    // LCID: https://msdn.microsoft.com/en-us/library/ms912047%28v=winembedded.10%29.aspx?f=255&MSPPError=-2147217396
                    WebTemplateCollection templates = web.GetAvailableWebTemplates(1033, false);
                    context.Load(templates);
                    //Execute the query to the server    
                    await context.ExecuteQueryAsync();
                    // Loop through all the list templates    
                    foreach (WebTemplate template in templates)
                    {
                        SharePointTemplate item = new SharePointTemplate() { ID = template.Id, Title = template.Title, Name = template.Name, Description = template.Description };
                        results.Add(item);
                    }
                }
            } catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
            return new OkObjectResult(results);
        }
    }
}