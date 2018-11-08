using BLL.Service;
using Models;
using System;
using System.Collections.Generic;
using PagedList;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcoCharge.Controllers
{
    public class EcosenseController : BaseController
    {
        // GET: Serial
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "serial_desc" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            using (var service = new Service<EcoSense>())
            {
                int id = (int)Session["UserId"];

                var comodos = service.GetRepository().Where(c => c.UsuarioId.Equals(id));

                if (!String.IsNullOrEmpty(searchString))
                {
                    comodos = comodos.Where(s => s.SerialAparelho.Serial.Contains(searchString));
                }

                switch (sortOrder)
                {
                    case "serial_desc":
                        comodos = comodos.OrderByDescending(s => s.SerialAparelho.Serial);
                        break;
                    default:
                        comodos = comodos.OrderBy(s => s.SerialAparelho.Serial);
                        break;
                }

                int pageSize = 5;
                int pageNumber = (page ?? 1);

                var lista = comodos.ToPagedList(pageNumber, pageSize);

                foreach (var item in lista)
                {
                    using (var serviceSerialAparelho = new Service<SerialAparelho>())
                    {
                        item.SerialAparelho = serviceSerialAparelho.FindById(item.SerialAparelho);
                    }
                }

                Session["ListaEcoSense"] = lista;
            }

            return View();
        }

        [HttpPost]
        public JsonResult Cadastrar(EcoSense model)
        {
            try
            {
                if (model == null)
                    throw new Exception("Preencha o serial");

                if (model.SerialAparelho == null)
                    throw new Exception("Preencha o serial");

                using (var service = new Service<EcoSense>())
                using (var serviceSerial = new Service<SerialAparelho>())
                {
                    var userId = Convert.ToInt32(Session["UserId"]);
                    model.UsuarioId = userId;
                    model.Ativo = false;

                    var serial = serviceSerial.GetRepository().Where(s => s.Serial.Equals(model.SerialAparelho.Serial)).FirstOrDefault();

                    if (serial == null)
                        throw new Exception("Serial não existe/incorreto");
                    
                    var list = service.GetRepository().ToList().Where(es => es.SerialAparelho.Serial.Equals(serial.Serial));

                    if (list.ToList().Count() != 0)
                        throw new Exception("Este serial ja foi cadastrado por outra pessoa.");

                    if (model.Id == 0)
                        service.Save(model);


                    TempData["Sucesso"] = true;
                    TempData["Mensagem"] = "Serial " + model.SerialAparelho.Serial + " cadastrado com sucesso";
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