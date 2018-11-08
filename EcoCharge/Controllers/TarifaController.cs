using BLL.Service;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcoCharge.Controllers
{
    public class TarifaController : BaseController
    {
        // GET: Tarifa
        public ActionResult Index()
        {
            using (var service = new Service<Usuario>()){
                var id_usuario = Convert.ToInt32(Session["UserId"]);

                var usuario = service.FindById(new Usuario() { Id = id_usuario });

                ViewBag.Tarifa = usuario.Tarifa;
            }
                
            return View();
        }

        [HttpPost]
        public ActionResult Cadastrar(string tarifa)
        {
            try
            {

                if (tarifa == null)
                    throw new Exception("Preencha o valor da tarifa");

                using (var service = new Service<Usuario>())
                {
                    var id_usuario = Convert.ToInt32(Session["UserId"]);

                    var usuario = service.FindById(new Usuario() { Id = id_usuario });

                    usuario.Tarifa = Convert.ToDecimal(tarifa.Replace('.', ','));

                    service.Save(usuario);

                    ViewBag.Tarifa = usuario.Tarifa;

                    TempData["Sucesso"] = true;
                    TempData["Mensagem"] = "Tarifa  " + usuario.Tarifa + " cadastrada com sucesso";
                }

            }
            catch (Exception exception)
            {
                TempData["Erro"] = true;
                TempData["Mensagem"] = exception.Message;
            }

            var view = RedirectToAction("Index");
            view.ExecuteResult(ControllerContext);

            return null;
        }

    }
}