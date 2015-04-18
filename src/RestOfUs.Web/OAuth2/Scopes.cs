using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestOfUs.Web.OAuth2 {
    public class Scope {
        private static Dictionary<string, string> scopes = new Dictionary<string, string>() {
            { "user-read-email", "Get your real email address" },
            { "user-read-birthday", "Get your real date of birth" },
            { "user-read-photos", "Access your photographs" }
        };

        public static string[] AllScopes {
            get { return (scopes.Keys.ToArray()); }
        }

        public static string Describe(string scope) {
            if (scopes.ContainsKey(scope)) return (scopes[scope]);
            throw (new ArgumentException("No such scope!", "scope"));
        }
    }
}