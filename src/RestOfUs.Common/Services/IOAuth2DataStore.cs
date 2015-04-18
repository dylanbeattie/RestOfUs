using RestOfUs.Common.Entities;
using RestOfUs.Common.Entities.OAuth2;

namespace RestOfUs.Common.Services {
    public interface IOAuth2DataStore {
        ClientApplication FindClient(string clientId);
        
        ClientAuthorization FindAuthorization(string code);
        void SaveAuthorization(ClientAuthorization authorization);

        RefreshToken FindRefreshToken(string token);
        void SaveRefreshToken(RefreshToken token);
    }
}