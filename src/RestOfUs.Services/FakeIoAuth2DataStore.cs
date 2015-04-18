using System.Collections.Generic;
using System.Linq;
using System.Net;
using RestOfUs.Common.Entities;
using RestOfUs.Common.Entities.OAuth2;
using RestOfUs.Common.Services;

namespace RestOfUs.Services {
    public class FakeIoAuth2DataStore : IOAuth2DataStore {
        private static readonly List<ClientApplication> Clients = new List<ClientApplication>();
        private static readonly List<ClientAuthorization> Authorizations = new List<ClientAuthorization>();
        private static readonly List<RefreshToken> RefreshTokens = new List<RefreshToken>(); 

        static FakeIoAuth2DataStore() {
            Clients.Add(new ClientApplication("43616eee", "5d5a0357c5e8", "Dylan's Demo App"));
        }

        public ClientApplication FindClient(string clientId) {
            return (Clients.FirstOrDefault(client => client.ClientId == clientId));
        }

        public ClientAuthorization FindAuthorization(string code) {
            return (Authorizations.FirstOrDefault(auth => auth.Code == code));
        }

        public void SaveAuthorization(ClientAuthorization authorization) {
            var existingAuthorization = FindAuthorization(authorization.Code);
            if (existingAuthorization != null) Authorizations.Remove(existingAuthorization);
            if (Authorizations.Any(auth => auth.Code == authorization.Code))
                Authorizations.Remove(existingAuthorization);
            Authorizations.Add(authorization);
        }

        public RefreshToken FindRefreshToken(string token) {
            return (RefreshTokens.FirstOrDefault(record => record.Token == token));
        }

        public void SaveRefreshToken(RefreshToken token) {
            var existingToken = FindRefreshToken(token.Token);



        }
    }
}