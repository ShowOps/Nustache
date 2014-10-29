﻿using System.Web.Mvc;
using System.Web.Routing;

namespace Nustache.Mvc5.Example
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected Wax _wax;

        protected void Application_Start()
        {
            _wax = new Wax();

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            RegisterViewEngines(ViewEngines.Engines);
        }

        protected void RegisterViewEngines(ViewEngineCollection engines)
        {
            engines.RemoveAt(0);

            // Create the engine and set up an event handler if desired.
            var engine = new NustacheViewEngine { RootContext = NustacheViewEngineRootContext.Model };
            engine.AdditionalProcessing += new AdditionalProcessingHandler(_wax.AdditionalProcessing);

            engines.Add(engine);
        }
    }
}
