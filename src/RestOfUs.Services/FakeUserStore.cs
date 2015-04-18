using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestOfUs.Common.Entities;

namespace RestOfUs.Services {
    public class FakeUserStore : IUserStore {
        private static readonly List<User> Users = new List<User>();

        static FakeUserStore() {
            Users.Add(new User { Username = "alice" });
            Users.Add(new User { Username = "bryan" });
            Users.Add(new User { Username = "carol" });
            Users.Add(new User { Username = "dylan" });
            Users.Add(new User { Username = "eddie" });
        }

        public User FindUserByUsername(string username) {
            return (Users.FirstOrDefault(user => user.Username == username));
        }
    }
}
