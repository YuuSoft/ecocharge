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

            try {

                if (model == null)
                    throw new Exception("Preencha o nome do cômodo");

                if (model.Nome == null)
                    throw new Exception("Preencha o nome do cômodo");

                using (var service = new Service<Comodo>())
                {
                    var userId = Convert.ToInt32(Session["UserId"]);
                    model.UsuarioId = userId;

                    service.Save(model);
                }

                TempData["Sucesso"] = true;
                TempData["Mensagem"] = "Comodo " + model.Nome + " cadastrado com sucesso";

            }
            catch (Exception exception) {
                TempData["Erro"] = true;
                TempData["Mensagem"] = exception.Message;
            }

            var view = RedirectToAction("Index");
            view.ExecuteResult(ControllerContext);

            return null;
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
                    }
                    else
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
            try
            {
                using (var service = new Service<Comodo>())
                {
                    if (id != null && id != 0)
                        throw new Exception("Informe o id!");

                    var userId = Convert.ToInt32(Session["UserId"]);
                    var model = service.GetRepository().Where(comodo => comodo.UsuarioId == userId && comodo.Id == id).FirstOrDefault();

                    if (model == null)
                        throw new Exception("Esse cômodo não existe!");

                    service.Delete(model);

                    TempData["Sucesso"] = true;
                    TempData["Mensagem"] = "Comodo removido com sucesso";
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