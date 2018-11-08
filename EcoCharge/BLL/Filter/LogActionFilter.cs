using BLL.Service;
using EcoCharge.Controllers;
using Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BLL.Filter
{
    public class LogActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string controller = (filterContext.RouteData.Values["controller"] as string);
            string action = (filterContext.RouteData.Values["action"] as string);
            bool userLogado = HttpContext.Current.Session["UserLogado"] != null ? (bool)HttpContext.Current.Session["UserLogado"] : false;
            bool adminLogado = HttpContext.Current.Session["AdminLogado"] != null ? (bool)HttpContext.Current.Session["AdminLogado"] : false;

            if (adminLogado)
            {
                //Sem Filtro
            }
            else if (userLogado)
            {
                if (controller == "Admin")
                {
                    if (action == "Index" || action == "Login")
                    {
                        //pode
                    }
                    else
                    {
                        filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                        this.RedirectToRoute(filterContext, new { controller = "Admin", action = "Login" });
                    }
                } else
                {
                    //pode
                }
            }
            else
            {
                if ((controller != "Account" && controller != "Home" && controller != "Error"))
                {
                    if (controller == "Admin")
                    {
                        if (action == "Index" || action == "Login")
                        {
                            //pode
                        } else
                        {
                            filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                            this.RedirectToRoute(filterContext, new { controller = "Admin", action = "Login" });
                        }
                    } else
                    {
                        filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                        this.RedirectToRoute(filterContext, new { controller = "Error", action = "NotAllowed" });
                    }
                }
            }


            //if (HttpContext.Current.Session["UserLogado"] != null || )
            //    if (Convert.ToBoolean(HttpContext.Current.Session["UserLogado"]) == true)
            //    {
            //        Log(filterContext.RouteData);

            //        if ()
            //    }
            //else
            //{
            //    if (!(controller.Equals("Account") ||
            //       controller.Equals("Home") ||
            //       controller.Equals("Error")) ||
            //       !(controller.Equals("Admin") && (action.Equals("Login")) || action.Equals("Index")))
            //        {

            //    } else
            //    {
            //        filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            //        this.RedirectToRoute(filterContext, new { controller = "Error", action = "NotAllowed" });
            //    }
            //}
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //Log(filterContext.RouteData);
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            //Log(filterContext.RouteData);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            //Log(filterContext.RouteData);
        }

        private void RedirectToRoute(ActionExecutingContext context, object routeValues)
        {
            var rc = new RequestContext(context.HttpContext, context.RouteData);
            var virtualPathData = RouteTable.Routes.GetVirtualPath(rc, new RouteValueDictionary(routeValues));
            if (virtualPathData != null)
            {
                string url = virtualPathData.VirtualPath;
                context.HttpContext.Response.Redirect(url, true);
            }
        }

        private void Log(RouteData routeData)
        {
            var log = new Log();
            log.Acao = Convert.ToString(routeData.Values["action"]);
            log.Controlador = Convert.ToString(routeData.Values["controller"]);
            log.Email = Convert.ToString(HttpContext.Current.Session["UserEmail"]);
            log.Ip = HttpContext.Current.Request.UserHostAddress;

            using (var service = new Service<Log>())
            {
                log = service.Save(log);

                var message = String.Format("Controller: {0}, Ação: {1}, Email: {2}, Ip: {3}", log.Acao, log.Controlador, log.Email, log.Ip);

                Debug.WriteLine(message, "Filtro de Ação");
            }
        }
    }
}