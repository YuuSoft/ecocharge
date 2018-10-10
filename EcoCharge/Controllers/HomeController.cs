using BLL.Filter;
using BLL.Service;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcoCharge.Controllers
{
    public class HomeController : BaseController
    {
        [OutputCache(Duration = 10)]
        public ActionResult Index()
        {

            /*TESTAR SE TA TUDO FUNCIONANDO*/

            //var service = new Service<Usuario>();

            //var usuarioNovo = new Usuario();
            //usuarioNovo.Id = 0;
            //usuarioNovo.Senha = "SenhaTeste123";
            //usuarioNovo.GoogleId = "blahablab";
            //usuarioNovo.Email = "luis_felipe_1998@hotmail.com";

            //usuarioNovo = service.Save(usuarioNovo);

            //var usuarioVelho = service.FindById(usuarioNovo);

            /*TESTADO E TA FUNCIONANDO LEGAL EH ISTO*/

            return View();
        }
    }
}