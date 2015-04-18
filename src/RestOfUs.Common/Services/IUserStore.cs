using RestOfUs.Common.Entities;

namespace RestOfUs.Common.Services {
    public interface IUserStore {
        User FindUserByUsername(string username);
    }
}