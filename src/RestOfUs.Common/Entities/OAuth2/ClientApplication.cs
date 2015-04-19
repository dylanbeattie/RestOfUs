using System;

namespace RestOfUs.Common.Entities.OAuth2 {
    public class ClientApplication {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Name { get; set; }

        public bool ValidateSecret(string clientSecret) {
            return (this.ClientSecret == clientSecret);
        }

        public ClientApplication(string clientId, string clientSecret, string name) {
            ClientId = clientId;
            ClientSecret = clientSecret;
            Name = name;
        }
    }

    public abstract class OAuth2Token {
        public string ClientId { get; set; }
        public string Username { get; set; }
        public string[] Scopes { get; set; }
        public DateTimeOffset ExpiresAt { get; set; }
    }

    public class AccessToken : OAuth2Token { }

    public class RefreshToken : OAuth2Token {
        public string Token { get; set; }
        public DateTimeOffset? RevokedAt { get; set; }
    }

    public class ClientAuthorization {
        public string Code { get; private set; }
        public string ClientId { get; private set; }
        public string Username { get; private set; }
        public string RedirectUri { get; private set; }
        public DateTimeOffset Expires { get; private set; }
        public string[] Scope { get; private set; }

        public bool Expired {
            get { return (Expires < DateTimeOffset.Now); }
        }

        public ClientAuthorization(string clientId, string username, string redirectUri, params string[] scope) {
            this.Code = Guid.NewGuid().ToString("N");
            this.ClientId = clientId;
            this.Username = username;
            this.RedirectUri = redirectUri;
            this.Scope = scope;
            this.Expires = DateTimeOffset.Now.AddMinutes(1);
        }

        public object CreateRefreshToken() {
            var token = new RefreshToken() {
                Token = Guid.NewGuid().ToString("N"),
                ClientId = this.ClientId,
                Username = this.Username,
                Scopes = this.Scope,
                ExpiresAt = DateTimeOffset.Now.AddYears(1)
            };
            return (token);
        }
    }
}
