using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace RestOfUs.Web.Services {
    public interface IAuthenticator {
        void SignOut();
        void SetAuthCookie(string username, bool rememberMe);
    }

    public class FormsAuthenticator : IAuthenticator {
        public void SignOut() {
            FormsAuthentication.SignOut();
        }
        public void SetAuthCookie(string username, bool rememberMe) {
            FormsAuthentication.SetAuthCookie(username, rememberMe);
        }
    }
}