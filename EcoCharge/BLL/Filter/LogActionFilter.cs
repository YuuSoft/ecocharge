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
            if (HttpContext.Current.Session["UserLogado"] != null)
                if (Convert.ToBoolean(HttpContext.Current.Session["UserLogado"]) == true)
                {
                    Log(filterContext.RouteData);
                } else
                {
                    //não é pra cair aqui mas ta aqui ne. SÓ PRA TE MESMO
                }
            else
            {
                if(!((filterContext.RouteData.Values["controller"] as string).Equals("Account") || 
                   (filterContext.RouteData.Values["controller"] as string).Equals("Home") ||
                   (filterContext.RouteData.Values["controller"] as string).Equals("Error")))
                {
                    filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
                    this.RedirectToRoute(filterContext, new { controller = "Error", action = "NotAllowed" });
                }
            }

            //Caso alguem tente acessar as páginas de error, redireciona para a pagina de erro 404.
            //if ((filterContext.RouteData.Values["controller"] as string).Equals("Error") &&
            //    !(filterContext.RouteData.Values["action"] as string).Equals("NotFound")) {
            //    this.RedirectToRoute(filterContext, new { controller = "Error", action = "NotFound" });
            //}

            //Regitra no log a atvidade do usuario, futuramente colocar o email do usuario junto.
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

                var message = String.Format("Controller: {0}, Ação: {1}, Email: {2}", log.Acao, log.Controlador, log.Email);

                Debug.WriteLine(message, "Filtro de Ação");
            }
        }
    }
}