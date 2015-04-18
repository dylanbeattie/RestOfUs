using System;
using System.Net;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using RelyingAppDemo.Controllers;

namespace RelyingAppDemo.OAuth2 {
    public class OAuth2Client {
        protected static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings {
            ContractResolver = new UnderscoreContractResolver()
        };

        public string ClientId {
            get { return (clientId); }
        }

        public string ClientSecret {
            get { return (clientSecret); }
        }

        public string TokenUri {
            get { return (tokenUri); }
        }
        private readonly string authorizeUri;
        private readonly string clientId;
        private readonly string clientSecret;
        private readonly string tokenUri;

        public OAuth2Client(string authorizeUri, string tokenUri, string clientId, string clientSecret) {
            this.authorizeUri = authorizeUri;
            this.tokenUri = tokenUri;
            this.clientId = clientId;
            this.clientSecret = clientSecret;
        }

        public Uri CreateAuthorizeUri(Uri redirectUri, string state, params string[] scopes) {
            var query = HttpUtility.ParseQueryString(String.Empty);
            query["response_type"] = "code";
            query["client_id"] = clientId;
            query["redirect_uri"] = redirectUri.AbsoluteUri;
            query["scope"] = String.Join(" ", scopes);
            query["state"] = state;
            var uri = new UriBuilder(authorizeUri) { Query = query.ToString() };
            return (uri.Uri);
        }

        public OAuth2Tokens ExchangeCodeForTokens(string code, Uri redirectUri) {
            using (var client = new WebClient()) {
                var form = HttpUtility.ParseQueryString(String.Empty);
                form["code"] = code;
                form["redirect_uri"] = redirectUri.AbsoluteUri;
                form["grant_type"] = "authorization_code";
                client.AuthorizeBasic(clientId, clientSecret);
                byte[] response = client.UploadValues(tokenUri, form);
                string json = Encoding.UTF8.GetString(response);
                var tokens = JsonConvert.DeserializeObject<OAuth2Tokens>(json, JsonSettings);
                return (tokens);
            }
        }

        public OAuth2Tokens RefreshToken(string refreshToken) {
            using (var client = new WebClient()) {
                var form = HttpUtility.ParseQueryString(String.Empty);
                form["grant_type"] = "refresh_token";
                form["refresh_token"] = refreshToken;
                client.AuthorizeBasic(clientId, clientSecret);
                var response = client.UploadValues(tokenUri, form);
                var json = Encoding.UTF8.GetString(response);
                var tokens = JsonConvert.DeserializeObject<OAuth2Tokens>(json, JsonSettings);
                return (tokens);
            }
        }
    }
}