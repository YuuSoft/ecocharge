using BLL.Service;
using Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcoCharge.Controllers
{
    public class ComodoController : BaseController
    {
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            using (var service = new Service<Comodo>())
            {
                int id = (int)Session["UserId"];

                var comodos = service.GetRepository().Where(c => c.UsuarioId.Equals(id));

                if (!String.IsNullOrEmpty(searchString))
                {
                    comodos = comodos.Where(s => s.Nome.Contains(searchString));
                }

                switch (sortOrder)
                {
                    case "name_desc":
                        comodos = comodos.OrderByDescending(s => s.Nome);
                        break;
                    default:
                        comodos = comodos.OrderBy(s => s.Nome);
                        break;
                }

                int pageSize = 5;
                int pageNumber = (page ?? 1);

               var lista = comodos.ToPagedList(pageNumber, pageSize);

                ViewData["ListaComodo"] = lista;
            }

            return View();
        }

        [HttpPost]
        public ActionResult Cadastrar(Comodo model)
        {
            var success = true;
            var msg = string.Empty;

            if (ModelState.IsValid)
            {
                using (var service = new Service<Comodo>())
                {
                    try
                    {
                        var userId = Convert.ToInt32(Session["UserId"]);
                        model.UsuarioId = userId;

                        service.Save(model);

                        msg = model.Nome + " cadastrado com sucesso";
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
                msg = "Preencha o nome do cômodo";
            }

            return Json(new { Success = success, Msg = msg }, "application/json", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Editar(int? id)
        {
            var success = true;
            var nome = string.Empty;

            if (id != null && id != 0)
            {
                using (var service = new Service<Comodo>())
                {
                    var userId = Convert.ToInt32(Session["UserId"]);
                    var model = service.GetRepository().Where(comodo => comodo.UsuarioId == userId && comodo.Id == id).FirstOrDefault();

                    if (model != null)
                    {
                        nome = model.Nome;
                    } else
                    {
                        success = false;
                    }
                }
            }

            return Json(new { Success = success, Id = id, Nome = nome }, "application/json", JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        public ActionResult Remover(int? id)
        {
            var success = true;
            var msg = string.Empty;

            if (id != null && id != 0)
            {
                using (var service = new Service<Comodo>())
                {
                    try
                    {
                        var userId = Convert.ToInt32(Session["UserId"]);
                        var model = service.GetRepository().Where(comodo => comodo.UsuarioId == userId && comodo.Id == id).FirstOrDefault();

                        if (model != null)
                        {
                            model = service.FindById(model);
                            service.Delete(model);
                            msg = "Removido com sucesso";
                        }
                        else
                        {
                            msg = "Esse comodo não existe";
                            success = false;
                        }
                    } catch (Exception ex)
                    {
                        success = false;
                        msg = ex.Message;
                    }

                }
            }

            return Json(new { Success = success, Msg = msg }, "application/json", JsonRequestBehavior.AllowGet);
        }
    }
}