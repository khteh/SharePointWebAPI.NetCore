using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.NetCore.Models
{
    /// <summary>
    /// SharePoint controller action input parameter
    /// </summary>
    public class SharePointParam
    {
        /// <summary>
        /// Site collection URL
        /// </summary>
        [Required]
        public string SiteCollectionURL { get; set; }
        /// <summary>
        /// Title of site or site collection
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Tenant admin URL
        /// </summary>
        public string TenantAdminURL { get; set; }
        /// <summary>
        /// Depending on use case, this could be just URL section instead of absolute URL.
        /// </summary>
        [Required]
        public string URL { get; set; }
        /// <summary>
        /// Template used by the site collection
        /// </summary>
        public string Template { get; set; }
        /// <summary>
        /// Description of site collection or site
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Owner of site / site collection
        /// </summary>
        public string Owner { get; set; }
        public long StorageMaximumLevel { get; set; }
        public double UserCodeMaximumLevel { get; set; }
    }
}