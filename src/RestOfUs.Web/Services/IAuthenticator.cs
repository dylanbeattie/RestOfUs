using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using LightInject;
using RestOfUs.Common.Entities.OAuth2;

namespace RestOfUs.Web.Services {
    public interface IAuthenticator {
        void SignOut();
        void SetAuthCookie(string username, bool rememberMe);
        string EncryptAccessToken(AccessToken token);
        AccessToken DecryptAccessToken(string encryptedToken);
    }

    public class FormsAuthenticator : IAuthenticator {
        public void SignOut() {
            FormsAuthentication.SignOut();
        }
        public void SetAuthCookie(string username, bool rememberMe) {
            FormsAuthentication.SetAuthCookie(username, rememberMe);
        }

        public string EncryptAccessToken(AccessToken token) {
            var userData = token.ClientId + " " + String.Join(" ", token.Scopes);
            var ticket = new FormsAuthenticationTicket(1, token.Username, DateTime.Now, token.ExpiresAt.LocalDateTime, false, userData);
            return (FormsAuthentication.Encrypt(ticket));
        }

        public AccessToken DecryptAccessToken(string encryptedToken) {
            var ticket = FormsAuthentication.Decrypt(encryptedToken);
            if (ticket == null)
                throw (new ArgumentException("Error decrypting supplied access token", "encryptedToken"));
            var scopes = ticket.UserData.Split(' ');
            var token = new AccessToken() {
                Username = ticket.Name,
                ClientId = scopes[0],
                ExpiresAt = new DateTimeOffset(ticket.Expiration),
                Scopes = scopes.Skip(1).ToArray()
            };
            return (token);
        }
    }
}