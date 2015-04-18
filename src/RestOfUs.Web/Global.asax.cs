using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using LightInject;
using RestOfUs.Services;
using RestOfUs.Web.Services;

namespace RestOfUs.Web {
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication {

        private static ServiceContainer container;

        protected void Application_Start() {
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            container = new ServiceContainer();
            container.RegisterControllers();
            container.Register<IUserStore, FakeUserStore>();
            container.Register<IAuthenticator, FormsAuthenticator>();
            container.EnableMvc();
        }

    }
}