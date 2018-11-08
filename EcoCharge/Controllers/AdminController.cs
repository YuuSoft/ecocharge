using BLL.Service;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace EcoCharge.Controllers
{
    public class AdminController : BaseController
    {

        // ROTAS =====================
        [HttpGet]
        public ActionResult Index()
        {
            return View("Login");
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult Usuarios(string searchString, string currentFilter, int? page)
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            using (var service = new Service<Usuario>())
            {
                var usuarios = service.GetRepository();

                if (!String.IsNullOrEmpty(searchString))
                {
                    usuarios = usuarios.Where(u => u.Nome.Contains(searchString) || u.Sobrenome.Contains(searchString));
                }

                int pageSize = 30;
                int pageNumber = (page ?? 1);

                var lista = usuarios.ToPagedList<Usuario>(pageNumber, pageSize);

                Session["ListaUsuarios"] = lista;
            }

            return View();
        }

        [HttpGet]
        public ActionResult Aparelhos()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Seriais()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Medidores()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Comodos()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Logs()
        {
            return View();
        }

        // ROTAS =====================


        // AÇÕES =====================
        [HttpPost]
        public ActionResult Login(string usuario, string senha)
        {
            RedirectToRouteResult view;
            
            if (usuario.Equals("admin") && senha.Equals("123")) //login e senha do adm
            {
                Session["AdminLogado"] = true;
                TempData["Sucesso"] = true;
                TempData["Mensagem"] = "Bem vindo, Admin.";
                view = RedirectToAction("Dashboard");
            } else
            {
                TempData["Erro"] = true;
                TempData["Mensagem"] = "Senha incorreta. Tente novamente.";
                view = RedirectToAction("Index");
            }

            view.ExecuteResult(ControllerContext);

            return null;
        }

        [HttpPost]
        public ActionResult Logout()
        {
            RedirectToRouteResult view;
            
            Session["AdminLogado"] = false;

            TempData["Sucesso"] = true;
            TempData["Mensagem"] = "Deslogado com sucesso.";

            view = RedirectToAction("Index");
            view.ExecuteResult(ControllerContext);

            return null;
        }

        [HttpPost]
        public ActionResult UsuariosSave(Usuario usuario)
        {
            try
            {
                using (var service = new Service<Usuario>())
                {
                    service.Save(usuario);
                    
                    TempData["Sucesso"] = true;
                    TempData["Mensagem"] = "Cadastrado com sucesso.";

                }
            } catch (Exception ex)
            {
                TempData["Erro"] = true;
                TempData["Mensagem"] = "Não foi possível cadastrar. Erro: " + ex.Message;
            }

            var view = RedirectToAction("Usuarios");
            view.ExecuteResult(ControllerContext);

            return null;
        }

        [HttpPost]
        public ActionResult AparelhosSave(Aparelho aparelho)
        {
            try
            {
                using (var service = new Service<Aparelho>())
                {
                    service.Save(aparelho);

                    TempData["Sucesso"] = true;
                    TempData["Mensagem"] = "Cadastrado com sucesso.";

                }
            }
            catch (Exception ex)
            {
                TempData["Erro"] = true;
                TempData["Mensagem"] = "Não foi possível cadastrar. Erro: " + ex.Message;
            }

            var view = RedirectToAction("Aparelhos");
            view.ExecuteResult(ControllerContext);

            return null;
        }

        [HttpPost]
        public ActionResult SeriaisSave(SerialAparelho serial)
        {
            try
            {
                using (var service = new Service<SerialAparelho>())
                {
                    service.Save(serial);

                    TempData["Sucesso"] = true;
                    TempData["Mensagem"] = "Cadastrado com sucesso.";

                }
            }
            catch (Exception ex)
            {
                TempData["Erro"] = true;
                TempData["Mensagem"] = "Não foi possível cadastrar. Erro: " + ex.Message;
            }

            var view = RedirectToAction("Aparelhos");
            view.ExecuteResult(ControllerContext);

            return null;
        }

        [HttpPost]
        public ActionResult MedidoresSave(EcoSense ecosense)
        {
            try
            {
                using (var service = new Service<EcoSense>())
                {
                    service.Save(ecosense);

                    TempData["Sucesso"] = true;
                    TempData["Mensagem"] = "Cadastrado com sucesso.";

                }
            }
            catch (Exception ex)
            {
                TempData["Erro"] = true;
                TempData["Mensagem"] = "Não foi possível cadastrar. Erro: " + ex.Message;
            }

            var view = RedirectToAction("Aparelhos");
            view.ExecuteResult(ControllerContext);

            return null;
        }

        [HttpPost]
        public ActionResult ComodosSave(Comodo comodo)
        {
            try
            {
                using (var service = new Service<Comodo>())
                {
                    service.Save(comodo);

                    TempData["Sucesso"] = true;
                    TempData["Mensagem"] = "Cadastrado com sucesso.";

                }
            }
            catch (Exception ex)
            {
                TempData["Erro"] = true;
                TempData["Mensagem"] = "Não foi possível cadastrar. Erro: " + ex.Message;
            }

            var view = RedirectToAction("Aparelhos");
            view.ExecuteResult(ControllerContext);

            return null;
        }

        [HttpGet]
        public ActionResult UsuariosEdit(int? id)
        {
            try
            {
                using (var service = new Service<Usuario>())
                {
                    var usuario = service.FindById(new Usuario() { Id = id.Value });

                    var json = Json(new { usuario = usuario }, "application/json", JsonRequestBehavior.AllowGet);
                    json.MaxJsonLength = Int32.MaxValue;
                    json.ExecuteResult(ControllerContext);
                }
            }
            catch (Exception ex)
            {
                TempData["Erro"] = true;
                TempData["Mensagem"] = "Não foi possível editar. Erro: " + ex.Message;

                var view = RedirectToAction("Usuarios");
                view.ExecuteResult(ControllerContext);
            }

            return null;
        }

        [HttpGet]
        public ActionResult AparelhosEdit(int? id)
        {
            try
            {
                using (var service = new Service<Aparelho>())
                {
                    var aparelho = service.FindById(new Aparelho() { Id = id.Value });

                    var json = Json(new { aparelho = aparelho }, "application/json", JsonRequestBehavior.AllowGet);
                    json.MaxJsonLength = Int32.MaxValue;
                    json.ExecuteResult(ControllerContext);
                }
            }
            catch (Exception ex)
            {
                TempData["Erro"] = true;
                TempData["Mensagem"] = "Não foi possível editar. Erro: " + ex.Message;

                var view = RedirectToAction("Aparelhos");
                view.ExecuteResult(ControllerContext);
            }

            return null;
        }

        [HttpGet]
        public ActionResult SeriaisEdit(int? id)
        {
            try
            {
                using (var service = new Service<SerialAparelho>())
                {
                    var serial = service.FindById(new SerialAparelho() { Id = id.Value });

                    var json = Json(new { serial = serial }, "application/json", JsonRequestBehavior.AllowGet);
                    json.MaxJsonLength = Int32.MaxValue;
                    json.ExecuteResult(ControllerContext);
                }
            }
            catch (Exception ex)
            {
                TempData["Erro"] = true;
                TempData["Mensagem"] = "Não foi possível editar. Erro: " + ex.Message;

                var view = RedirectToAction("Seriais");
                view.ExecuteResult(ControllerContext);
            }

            return null;
        }

        [HttpGet]
        public ActionResult MedidoresEdit(int? id)
        {
            try
            {
                using (var service = new Service<EcoSense>())
                {
                    var ecosense = service.FindById(new EcoSense() { Id = id.Value });

                    var json = Json(new { ecosense = ecosense }, "application/json", JsonRequestBehavior.AllowGet);
                    json.MaxJsonLength = Int32.MaxValue;
                    json.ExecuteResult(ControllerContext);
                }
            }
            catch (Exception ex)
            {
                TempData["Erro"] = true;
                TempData["Mensagem"] = "Não foi possível editar. Erro: " + ex.Message;

                var view = RedirectToAction("Medidores");
                view.ExecuteResult(ControllerContext);
            }

            return null;
        }

        [HttpGet]
        public ActionResult ComodosEdit(int? id)
        {
            try
            {
                using (var service = new Service<Comodo>())
                {
                    var comodo = service.FindById(new Comodo() { Id = id.Value });

                    var json = Json(new { comodo = comodo }, "application/json", JsonRequestBehavior.AllowGet);
                    json.MaxJsonLength = Int32.MaxValue;
                    json.ExecuteResult(ControllerContext);
                }
            }
            catch (Exception ex)
            {
                TempData["Erro"] = true;
                TempData["Mensagem"] = "Não foi possível editar. Erro: " + ex.Message;

                var view = RedirectToAction("Comodos");
                view.ExecuteResult(ControllerContext);
            }

            return null;
        }

        [HttpGet]
        public ActionResult UsuariosDelete(int? id)
        {
            try
            {
                using (var service = new Service<Usuario>())
                {
                    var usuario = service.FindById(new Usuario() { Id = id.Value });
                    service.Delete(usuario);
                }

                TempData["Sucesso"] = true;
                TempData["Mensagem"] = "Removido com sucesso";
            } catch (Exception ex)
            {
                TempData["Erro"] = true;
                TempData["Mensagem"] = "Não foi possível remover. Erro: " + ex.Message;
            }

            var view = RedirectToAction("Usuarios");
            view.ExecuteResult(ControllerContext);

            return null;
        }

        [HttpGet]
        public ActionResult AparelhosDelete(int? id)
        {
            try
            {
                using (var service = new Service<Aparelho>())
                {
                    var aparelho = service.FindById(new Aparelho() { Id = id.Value });
                    service.Delete(aparelho);
                }

                TempData["Sucesso"] = true;
                TempData["Mensagem"] = "Removido com sucesso";
            }
            catch (Exception ex)
            {
                TempData["Erro"] = true;
                TempData["Mensagem"] = "Não foi possível remover. Erro: " + ex.Message;
            }

            var view = RedirectToAction("Aparelhos");
            view.ExecuteResult(ControllerContext);

            return null;
        }

        [HttpGet]
        public ActionResult SeriaisDelete(int? id)
        {
            try
            {
                using (var service = new Service<SerialAparelho>())
                {
                    var serial = service.FindById(new SerialAparelho() { Id = id.Value });
                    service.Delete(serial);
                }

                TempData["Sucesso"] = true;
                TempData["Mensagem"] = "Removido com sucesso";
            }
            catch (Exception ex)
            {
                TempData["Erro"] = true;
                TempData["Mensagem"] = "Não foi possível remover. Erro: " + ex.Message;
            }

            var view = RedirectToAction("Seriais");
            view.ExecuteResult(ControllerContext);

            return null;
        }

        [HttpGet]
        public ActionResult MedidoresDelete(int? id)
        {
            try
            {
                using (var service = new Service<EcoSense>())
                {
                    var ecosense = service.FindById(new EcoSense() { Id = id.Value });
                    service.Delete(ecosense);
                }

                TempData["Sucesso"] = true;
                TempData["Mensagem"] = "Removido com sucesso";
            }
            catch (Exception ex)
            {
                TempData["Erro"] = true;
                TempData["Mensagem"] = "Não foi possível remover. Erro: " + ex.Message;
            }

            var view = RedirectToAction("Medidores");
            view.ExecuteResult(ControllerContext);

            return null;
        }

        [HttpGet]
        public ActionResult ComodosDelete(int? id)
        {
            try
            {
                using (var service = new Service<Comodo>())
                {
                    var comodo = service.FindById(new Comodo() { Id = id.Value });
                    service.Delete(comodo);
                }

                TempData["Sucesso"] = true;
                TempData["Mensagem"] = "Removido com sucesso";
            }
            catch (Exception ex)
            {
                TempData["Erro"] = true;
                TempData["Mensagem"] = "Não foi possível remover. Erro: " + ex.Message;
            }

            var view = RedirectToAction("Comodos");
            view.ExecuteResult(ControllerContext);

            return null;
        }
    }
}