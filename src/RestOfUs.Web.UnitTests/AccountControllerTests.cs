using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using RestOfUs.Services;
using RestOfUs.Web.Controllers;
using RestOfUs.Web.Services;
using Shouldly;

namespace RestOfUs.Web.UnitTests {
    [TestFixture]
    public class AccountControllerTests {

        [Test]
        [TestCase("alice", "p@ssw0rd", true)]
        [TestCase("alice", "p@ssw0rd", false)]
        public void Login_Success_Returns_Account(string username, string password, bool remember) {
            var mockAuthenticator = new Mock<IAuthenticator>();
            var fakeUserStore = new FakeUserStore();
            var controller = new AccountController(fakeUserStore, mockAuthenticator.Object);
            mockAuthenticator.Setup(auth => auth.SetAuthCookie(username, remember)).Verifiable();
            controller.SignIn(username, password, remember);
            mockAuthenticator.Verify();
        }

        [Test]
        [TestCase("no.such.user", "p@ssw0rd", true)]
        [TestCase("no.such.user", "p@ssw0rd", false)]
        public void Login_Username_Not_Found_Returns_View(string username, string password, bool remember) {
            var mockAuthenticator = new Mock<IAuthenticator>();
            var fakeUserStore = new FakeUserStore();
            var controller = new AccountController(fakeUserStore, mockAuthenticator.Object);
            mockAuthenticator.Setup(auth => auth.SetAuthCookie(It.IsAny<string>(), It.IsAny<bool>()))
                .Callback(() => Assert.Fail("Non-existent username should not set auth cookie"));
            var result = controller.SignIn(username, password) as ViewResult;
            result.ShouldNotBe(null);
            mockAuthenticator.Verify();
            ((string)result.ViewBag.Message).ShouldBe("Username not found");
        }


        [Test]
        [TestCase("alice", "incorrect", true)]
        [TestCase("alice", "incorrect", false)]
        public void Login_Incorrect_Password_Returns_View(string username, string password, bool remember) {
            var mockAuthenticator = new Mock<IAuthenticator>();
            var fakeUserStore = new FakeUserStore();
            var controller = new AccountController(fakeUserStore, mockAuthenticator.Object);
            mockAuthenticator.Setup(auth => auth.SetAuthCookie(It.IsAny<string>(), It.IsAny<bool>()))
                .Callback(() => Assert.Fail("Incorrect password should not set auth cookie"));
            var result = controller.SignIn(username, password) as ViewResult;
            result.ShouldNotBe(null);
            mockAuthenticator.Verify();
            ((string)result.ViewBag.Message).ShouldBe("Incorrect password");
        }
    }
}
