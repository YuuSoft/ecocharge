using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcoCharge.Controllers
{
    public class ErrorController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NotFound(string aspxerrorpath)
        {
            Response.StatusCode = 404;
            return View("NotFound");
        }

        public ActionResult NotAllowed(string aspxerrorpath)
        {
            return View("NotFound");
        }
    }
}