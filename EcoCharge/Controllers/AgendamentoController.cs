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
    public class AgendamentoController : Controller
    {
        // GET: Agendamento
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

            using (var service = new Service<Agendamento>())
            {
                int id = (int)Session["UserId"];

                var agemdamentos = service.GetRepository().Where(c => c.UsuarioId.Equals(id));

                if (!String.IsNullOrEmpty(searchString))
                {
                    agemdamentos = agemdamentos.Where(s => s.Detalhes.Contains(searchString));
                }

                switch (sortOrder)
                {
                    case "name_desc":
                        agemdamentos = agemdamentos.OrderByDescending(s => s.Detalhes);
                        break;
                    default:
                        agemdamentos = agemdamentos.OrderBy(s => s.Detalhes);
                        break;
                }

                int pageSize = 5;
                int pageNumber = (page ?? 1);

                var lista = agemdamentos.ToPagedList(pageNumber, pageSize);

                ViewData["ListaAgendamentos"] = lista;
            }

            using (var service = new Service<EcoSense>())
            {
                int id = (int)Session["UserId"];

                var lista = service.GetRepository().Where(k => k.UsuarioId == id).ToList();

                ViewBag.ListaEcoSense = lista;

            }

            return View();
        }

        [HttpPost]
        public ActionResult Cadastrar(Agendamento model,String inicio, String final, String segunda)
        {

            try
            {

                if (model == null)
                    throw new Exception("Preencha o dados");
                if (model.EcoSenseId == 0)
                    throw new Exception("Preencha o EcoSense");
                if (model.Detalhes == null)
                    throw new Exception("Preencha os Detalhes");

                using (var service = new Service<Agendamento>())
                {
                    var userId = Convert.ToInt32(Session["UserId"]);
                    model.UsuarioId = userId;

                    service.Save(model);
                }

                TempData["Sucesso"] = true;
                TempData["Mensagem"] = "Agendamento cadastrado com sucesso";

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

        [HttpGet]
        public ActionResult Editar(int? id)
        {
            var success = true;
            var detalhes = string.Empty;
            var ecosenseid = 0;

            if (id != null && id != 0)
            {
                using (var service = new Service<Agendamento>())
                {
                    var userId = Convert.ToInt32(Session["UserId"]);
                    var model = service.GetRepository().Where(agendamento => agendamento.UsuarioId == userId && agendamento.Id == id).FirstOrDefault();

                    if (model != null)
                    {
                        detalhes = model.Detalhes;
                        ecosenseid = model.EcoSenseId;
                    }
                    else
                    {
                        success = false;
                    }
                }
            }

            return Json(new { Success = success, Id = id, Detalhes = detalhes }, "application/json", JsonRequestBehavior.AllowGet);
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
                    var model = service.GetRepository().Where(agendamento => agendamento.UsuarioId == userId && agendamento.Id == id).FirstOrDefault();

                    if (model == null)
                        throw new Exception("Esse agendamento não existe!");

                    service.Delete(model);

                    TempData["Sucesso"] = true;
                    TempData["Mensagem"] = "Agendamento removido com sucesso";
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