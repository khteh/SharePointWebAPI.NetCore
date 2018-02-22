using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.NetCore.Middleware
{
    public class BasicAuthentication
    {
        private readonly RequestDelegate _requestDelegate;
        public BasicAuthentication(RequestDelegate requestDelegate) => _requestDelegate = requestDelegate;
        public async Task Invoke(HttpContext context)
        {
            string header = context.Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(header) && header.StartsWith("Basic"))
            {
                // Extract credentials
                string encodedCredential = header.Substring("Basic ".Length).Trim();
                Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                string credential = encoding.GetString(Convert.FromBase64String(encodedCredential));
                int separatorIndex = credential.IndexOf(':');
                string username = credential.Substring(0, separatorIndex);
                string password = credential.Substring(separatorIndex + 1);
            }
        }
    }
}