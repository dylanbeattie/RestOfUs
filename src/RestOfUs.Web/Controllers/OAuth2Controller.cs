using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RestOfUs.Common.Entities.OAuth2;
using RestOfUs.Common.Services;
using RestOfUs.Web.Models;
using RestOfUs.Web.OAuth2;

// ReSharper disable InconsistentNaming
namespace RestOfUs.Web.Controllers {
    [Authorize]
    public class OAuth2Controller : Controller {
        private readonly IOAuth2DataStore oAuth2DataStore;

        public OAuth2Controller(IOAuth2DataStore oAuth2DataStore) {
            this.oAuth2DataStore = oAuth2DataStore;
        }

        public ActionResult Authorize(
            string response_type, string client_id, string redirect_uri, string state,
            string scope) {
            var client = oAuth2DataStore.FindClient(client_id);
            if (client == null) throw (new HttpException((int)HttpStatusCode.BadRequest, "Invalid client_id"));
            var scopes = scope.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var model = new AuthorizeScopesViewModel {
                ClientId = client_id,
                RedirectUri = redirect_uri,
                State = state,
                ClientAppName = client.Name,
                Username = User.Identity.Name,
                Scopes = scopes
            };
            return (View(model));
        }

        public ActionResult Token(string grant_type, string code, string redirect_uri, string refresh_token) {
            switch (grant_type) {
                case "authorization_code":
                    return (CreateTokens(code, redirect_uri));
                case "refresh_token":
                    return (RefreshToken(refresh_token));
                default:
                    throw (BadRequest("unsupported grant_type"));
            }
        }

        private ActionResult RefreshToken(string refreshToken) {
            throw new NotImplementedException();
        }

        private ActionResult CreateTokens(string code, string redirectUri) {
            var authorization = oAuth2DataStore.FindAuthorization(code);
            if (authorization == null) throw (BadRequest("invalid authorization code"));
            if (authorization.Expired) throw (BadRequest("authorization no longer valid"));
            var refreshToken = authorization.CreateRefreshToken();
            
        }

        public ActionResult Redirect(string response_type, string client_id, string redirect_uri, string state,
            string scope) {
            var scopes = scope.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var client = oAuth2DataStore.FindClient(client_id);
            if (client == null) throw (new HttpException((int)HttpStatusCode.BadRequest, "Invalid client_id"));
            var auth = new ClientAuthorization(client_id, User.Identity.Name, redirect_uri, scopes);
            oAuth2DataStore.SaveAuthorization(auth);
            var query = HttpUtility.ParseQueryString(String.Empty);
            query["code"] = auth.Code;
            query["state"] = state;
            var uri = new UriBuilder(redirect_uri) {
                Query = query.ToString()
            };
            var model = new RedirectViewModel() {
                RedirectUri = uri.Uri,
                Client = client,
                Authorization = auth
            };
            return (View(model));
        }

        private HttpException BadRequest(string message) {
            return (new HttpException((int)HttpStatusCode.BadRequest, message));
        }
    }
}
// ReSharper restore InconsistentNaming
