using System.Web.Mvc;
using RestOfUs.Services;
using RestOfUs.Web.Services;

namespace RestOfUs.Web.Controllers {
    [Authorize]
    public class AccountController : Controller {

        private readonly IUserStore userStore;
        private readonly IAuthenticator authenticator;

        public AccountController(IUserStore userStore, IAuthenticator authenticator) {
            this.userStore = userStore;
            this.authenticator = authenticator;
        }

        [AllowAnonymous]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult SignIn(string message = "Please log in") {
            ViewBag.Message = message;
            return (View());
        }

        public ActionResult SignOut() {
            authenticator.SignOut();
            return (RedirectToAction("Index"));
        }

        [AllowAnonymous]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SignIn(string username, string password, bool remember = false) {
            var user = userStore.FindUserByUsername(username);
            if (user == null) return (SignIn("Username not found"));
            if (user.PasswordMatches(password)) {
                authenticator.SetAuthCookie(username, remember);
                return (RedirectToAction("Index", "Account"));
            }
            return (SignIn("Incorrect password"));
        }

        public ActionResult Index() {
            return (View(User));
        }
    }
}