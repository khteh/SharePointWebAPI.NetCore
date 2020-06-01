using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharePointWebAPI.NetCore.Models.Request
{
    public class TokenRequest
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public TokenRequest() { }
        public TokenRequest(string name, string password) => (Name, Password) = (name, password);
    }
}