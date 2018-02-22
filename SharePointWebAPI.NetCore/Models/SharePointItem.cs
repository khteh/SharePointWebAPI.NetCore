using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.NetCore.Models
{
    public class SharePointItem
    {
        public string SiteCollectionURL { get; set; }
        public string Title { get; set; }
        public string URL { get; set; }
        public string Template { get; set; }
    }
}