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
    public class SharePointItem
    {
        [Required]
        public string SiteCollectionURL { get; set; }
        public string Title { get; set; }
        /// <summary>
        /// This is URL section. Not the whole url
        /// </summary>
        [Required]
        public string URL { get; set; }
        public string Template { get; set; }
        public string Description { get; set; }
    }
}