﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;

namespace Karma.REST.Http.Routing
{
    public class KarmaApiControllerActionSelector : ApiControllerActionSelector
    {
        public override HttpActionDescriptor SelectAction(HttpControllerContext controllerContext)
        {

            IHttpRouteData routeData = controllerContext.RouteData;
            bool containsAction = routeData.Values.ContainsKey("action");

            if (containsAction)
            {
                //---------------------------------------------------------------------------------
                //PUT OR DELETE HTTPVERB , TRY FIRST TO ENGAGE API DEFAULT MODEL (DELETE AND PUT, ALWAYS SEND AND ID)
                if (
                    (controllerContext.Request.Method.Method == "PUT" ||
                    controllerContext.Request.Method.Method == "DELETE") &&
                    routeData.Values["id"] == null
                    )
                {
                    routeData.Values["id"] = routeData.Values["action"];
                    routeData.Values["action"] = controllerContext.Request.Method.Method;
                }
                //---------------------------------------------------------------------------------

                //Otherwise call directly to verb or ChildActions
                return base.SelectAction(controllerContext);
            }

            try
            {
                routeData.Values["action"] = controllerContext.Request.Method.Method;

                return base.SelectAction(controllerContext);

            }
            finally
            {

                routeData.Values.Remove("action");
            }
        }
    }
}
