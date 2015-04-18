using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RestOfUs.Common.Entities.OAuth2;

namespace RestOfUs.Web.Models {
    public class AuthorizeScopesViewModel {
        public string ClientAppName { get; set; }
        public string Username { get; set; }
        public IEnumerable<string> Scopes { get; set; }
        public string RedirectUri { get; set; }
        public string State { get; set; }
        public string ClientId { get; set; }
    }

    public class RedirectViewModel {
        public ClientApplication Client { get; set; }
        public ClientAuthorization Authorization { get; set; }
        public Uri RedirectUri { get; set; }
    }
}