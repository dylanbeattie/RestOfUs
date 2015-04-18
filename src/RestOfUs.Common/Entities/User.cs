namespace RestOfUs.Common.Entities {
    public class User {
        private const string password = "p@ssw0rd";
        public string Username { get; set; }
        public bool PasswordMatches(string password) {
            return (User.password == password);
        }
    }
}