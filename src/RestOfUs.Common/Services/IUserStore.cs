using RestOfUs.Common.Entities;

namespace RestOfUs.Services {
    public interface IUserStore {
        User FindUserByUsername(string username);
    }
}