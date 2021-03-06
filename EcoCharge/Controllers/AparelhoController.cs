﻿using BLL.Service;
using Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcoCharge.Controllers
{
    public class AparelhoController : BaseController
    {
        // GET: Aparelho
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

            using (var service = new Service<Aparelho>())
            {
                int id = (int)Session["UserId"];

                var aparelhos = service.GetRepository().Where(c => c.UsuarioId.Equals(id));

                if (!String.IsNullOrEmpty(searchString))
                {
                    aparelhos = aparelhos.Where(s => s.Nome.Contains(searchString));
                }

                switch (sortOrder)
                {
                    case "name_desc":
                        aparelhos = aparelhos.OrderByDescending(s => s.Nome);
                        break;
                    default:
                        aparelhos = aparelhos.OrderBy(s => s.Nome);
                        break;
                }

                int pageSize = 5;
                int pageNumber = (page ?? 1);

                var lista = aparelhos.ToPagedList(pageNumber, pageSize);

                ViewData["ListaAparelho"] = lista;
            }

            using (var service = new Service<EcoSense>())
            using (var serviceComodo = new Service<Comodo>())
            {
                int id = (int)Session["UserId"];

                var lista = service.GetRepository().Where(k=>k.UsuarioId == id).ToList();

                ViewBag.ListaEcoSense = lista;

                var listaComodo = serviceComodo.GetRepository().Where(k => k.UsuarioId == id).ToList();

                ViewBag.ListaComodo = listaComodo;

                var view = View("Index");
                view.ExecuteResult(ControllerContext);
            }

            return null;
        }

        [HttpPost]
        public ActionResult Cadastrar(Aparelho model)
        {
            try
            {

                if (model.Nome == null)
                    throw new Exception("Preencha o nome do aparelho");

                if (model.Descricao == null)
                    throw new Exception("Preencha a descrição do aparelho");

                if (model.Voltagem == 0)
                    throw new Exception("Preencha a voltagem do aparelho");

                if (model.EcoSenseId == 0)
                    throw new Exception("Escolha o EcoSense do aparelho");

                if (model.ComodoId == 0)
                    throw new Exception("Escolha o cômodo do aparelho");

                if (model.Cor == null)
                    throw new Exception("Preencha a cor do aparelho");

                using (var service = new Service<Aparelho>())
                {
                    var userId = Convert.ToInt32(Session["UserId"]);
                    model.UsuarioId = userId;

                    service.Save(model);
                }

                TempData["Sucesso"] = true;
                TempData["Mensagem"] = "Aparelho " + model.Nome + " cadastrado com sucesso";

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
            var nome = string.Empty;
            var descricao = string.Empty;
            var voltagem = 0;
            var ecosenseid = 0;
            var comodoid = 0;
            var cor = string.Empty;

            if (id != null && id != 0)
            {
                using (var service = new Service<Aparelho>())
                {
                    var userId = Convert.ToInt32(Session["UserId"]);
                    var model = service.GetRepository().Where(aparelho => aparelho.UsuarioId == userId && aparelho.Id == id).FirstOrDefault();

                    if (model != null)
                    {
             
                        nome = model.Nome;
                        descricao = model.Descricao;
                        voltagem = model.Voltagem;
                        ecosenseid = model.EcoSenseId;
                        comodoid = model.ComodoId;
                        cor = model.Cor;

                    }
                    else
                    {
                        success = false;
                    }
                }
            }

            return Json(new { Success = success, Id = id, Nome = nome, Descricao = descricao, Voltagem = voltagem, Ecosenseid = ecosenseid, Comodoid = comodoid, Cor = cor}, "application/json", JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        public ActionResult Remover(int? id)
        {
            try
            {
                using (var service = new Service<Aparelho>())
                {
                    if (id != null && id != 0)
                        throw new Exception("Informe o id!");

                    var userId = Convert.ToInt32(Session["UserId"]);
                    var model = service.GetRepository().Where(aparelho => aparelho.UsuarioId == userId && aparelho.Id == id).FirstOrDefault();

                    if (model == null)
                        throw new Exception("O aparelho não existe!");

                    service.Delete(model);

                    TempData["Sucesso"] = true;
                    TempData["Mensagem"] = "Aparelho removido com sucesso";
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