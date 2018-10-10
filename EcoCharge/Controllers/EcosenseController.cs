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

                ViewData["ListaEcoSense"] = lista;
            }

            return View();
        }

        [HttpPost]
        public ActionResult Cadastrar(EcoSense model)
        {
            var success = true;
            var msg = string.Empty;

            if (model.SerialAparelho != null)
            {
                if (model.SerialAparelho.Serial != null)
                {
                    using (var service = new Service<EcoSense>())
                    using (var serviceSerial = new Service<SerialAparelho>())
                    {
                        try
                        {
                            var userId = Convert.ToInt32(Session["UserId"]);
                            model.UsuarioId = userId;
                            model.StatusAparelho = false;

                            var serial = serviceSerial.GetRepository().Where(s => s.Serial.Equals(model.SerialAparelho.Serial)).FirstOrDefault();

                            if (serial != null)
                            {
                                var list = service.GetRepository().ToList().Where(es => es.SerialAparelho.Equals(serial));

                                if (list.ToList().Count() == 0)
                                {
                                    if (model.Id == 0)
                                        service.Save(model);
                                }
                                else
                                    throw new Exception("Este serial ja foi cadastrado por outra pessoa.");
                            }
                            else
                                throw new Exception("Serial não existe/incorreto");

                            msg = "Serial " + model.SerialAparelho.Serial + " cadastrado com sucesso";
                        }
                        catch (Exception ex)
                        {
                            success = false;
                            msg = ex.Message;
                        }
                    }
                }
                else
                {
                    success = false;
                    msg = "Preencha o serial";
                }
            }
            else
            {
                success = false;
                msg = "Preencha o serial";
            }

            return Json(new { Success = success, Msg = msg }, "application/json", JsonRequestBehavior.AllowGet);
        }
    }
}