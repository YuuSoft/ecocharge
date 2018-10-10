using BLL.Service;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcoCharge.Controllers
{
    public class AccountController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Cadastro() {
            return View();
        }

        [HttpPost]
        public ActionResult Logar(Login model)
        {
            bool success = true;
            string msg = string.Empty;

            if (ModelState.IsValid)
            {
                using (var service = new Service<Usuario>())
                {
                    var user = service.GetRepository().Where(u => u.Email == model.email && u.Senha == model.password).FirstOrDefault();
                    var exists = user != null;

                    if (!exists)
                    {
                        success = false;
                        msg = "Email ou senha incorreto";
                    }
                    else
                    {
                        Session["UserLogado"] = true;
                        Session["UserEmail"] = user.Email;
                        Session["UserId"] = user.Id;
                        
                        msg = "/";
                    }
                }
            } else
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                                           .Where(y => y.Count > 0)
                                           .FirstOrDefault();

                success = false;
                msg = errors.ElementAt(0).ErrorMessage;
            }
            
            Response.StatusCode = 200;
            return Json(new { Success = success, Msg = msg}, "application/json", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Deslogar()
        {
            bool success = true;
            string msg = string.Empty;

            if (Session["UserLogado"] != null)
            {
                Session["UserLogado"] = null;
                Session["UserEmail"] = null;
                Session["UserId"] = null;

                msg = "Deslogado com sucesso";
            }

            Response.StatusCode = 200;
            return Json(new { Success = success, Msg = msg }, "application/json", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Registrar(Login model, string passwordCheck)
        {
            bool success = true;
            string msg = string.Empty;

            if (ModelState.IsValid) {
                if (!model.password.Equals(passwordCheck))
                {
                    success = false;
                    msg = "As senhas não são iguais";
                }
                else if (model.password.Length < 8)
                {
                    success = false;
                    msg = "Senha deve ter no minimo 8 caracteres";
                }
                else {
                    try
                    {
                        var addr = new System.Net.Mail.MailAddress(model.email);
                        var b = addr.Address == model.email;
                    }
                    catch
                    {
                        success = false;
                        msg = "Email invalido";
                    }
                }
            }
            else {
                var errors = ModelState.Select(x => x.Value.Errors)
                                           .Where(y => y.Count > 0)
                                           .FirstOrDefault();

                success = false;
                msg = errors.ElementAt(0).ErrorMessage;
            }

            if (success)
                using (var service = new Service<Usuario>())
                {
                    bool exists = service.GetRepository().Where(u => u.Email == model.email).FirstOrDefault() != null;

                    if (exists)
                    {
                        success = false;
                        msg = "Já existe um usuário com este email";
                    }
                    else
                    {
                        Usuario user = new Usuario();
                        user.Email = model.email;
                        user.Senha = model.password;

                        try {
                            service.Save(user);

                            msg = "/Account/";
                        } catch (Exception ex)
                        {
                            success = false;
                            msg = ex.Message;
                        }
                    }
                }

            Response.StatusCode = 200;
            return Json(new { Success = success, Msg = msg }, "application/json", JsonRequestBehavior.AllowGet);
        }
    }
}