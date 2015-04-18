using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using RelyingAppDemo.OAuth2;

namespace RelyingAppDemo.Controllers {
    public class HomeController : Controller {

        public ActionResult Index() {
            return (View());
        }

        private readonly OAuth2Client oauth2 = new OAuth2Client(
            "http://www.restofus.local/oauth2/authorize",
            "http://www.restofus.local/oauth2/token",
            "43616eee",
            "5d5a0357c5e8"
        );

        public ActionResult Authorize() {
            var redirectUri = new Uri(Url.Action("Callback", "Home", null, Request.Url.Scheme));
            var state = Session.SessionID;
            var authorizeUri = oauth2.CreateAuthorizeUri(redirectUri, state, "user-read-email", "user-read-birthday", "user-read-photos");
            return (View(authorizeUri));
        }

        public ActionResult Callback(string code, string state) {
            if (state != Session.SessionID)
                return (new HttpStatusCodeResult(HttpStatusCode.BadRequest, "callback failed: the state parameter did not match the state supplied in the original redirect."));
            var redirectUri = new Uri(Url.Action("Callback", "Home", null, Request.Url.Scheme));
            var tokens = oauth2.ExchangeCodeForTokens(code, redirectUri);
            return (View(tokens));
        }
    }
}
