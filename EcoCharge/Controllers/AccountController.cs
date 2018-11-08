using BLL.Service;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Helpers;
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
        public ActionResult Cadastro()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Senha()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Logar(Login model)
        {
            try
            {

                using (var service = new Service<Usuario>())
                {
                    var user = service.GetRepository().Where(u => u.Email == model.email && u.Senha == model.password).FirstOrDefault();
                    var exists = user != null;

                    if (!exists)
                    {
                        throw new Exception("Email ou senha incorreto");
                    }
                    else
                    {
                        Session["UserLogado"] = true;
                        Session["UserEmail"] = user.Email;
                        Session["UserNome"] = user.Nome;
                        Session["UserSobrenome"] = user.Sobrenome;
                        Session["UserSpace"] = " ";
                        Session["UserFoto"] = user.Foto;
                        Session["UserId"] = user.Id;
                    }
                }

                TempData["Sucesso"] = true;
                TempData["Mensagem"] = "Seja Bem Vindo " + model.email;

                var view = RedirectToAction("Index", "Painel");
                view.ExecuteResult(ControllerContext);

            }
            catch (Exception exception)
            {
                TempData["Erro"] = true;
                TempData["Mensagem"] = exception.Message;

                var view = RedirectToAction("Index");
                view.ExecuteResult(ControllerContext);
            }

            return null;
        }

        [HttpPost]
        public ActionResult Deslogar()
        {
            try {
                if (Session["UserLogado"] != null)
                {
                    Session["UserLogado"] = false;
                    Session["UserEmail"] = null;
                    Session["UserId"] = null;
                    Session["UserNome"] = null;
                    Session["UserSobrenome"] = null;
                    Session["UserFoto"] = null;

                }

                TempData["Sucesso"] = true;
                TempData["Mensagem"] = "Usuario Deslogado";
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

        [HttpPost]
        public ActionResult Registrar(Login model, string passwordCheck)
        {
            try
            {

                if (!model.password.Equals(passwordCheck))
                    throw new Exception("As senhas não são iguais");

                if (model.password.Length < 8)
                    throw new Exception("Senha deve ter no minimo 8 caracteres");

                var addr = new System.Net.Mail.MailAddress(model.email);
                var b = addr.Address == model.email;

                using (var service = new Service<Usuario>())
                {
                    bool exists = service.GetRepository().Where(u => u.Email == model.email).FirstOrDefault() != null;

                    if (exists)
                        throw new Exception("Já existe um usuário com este email");

                    Usuario user = new Usuario();
                    user.Email = model.email;
                    user.Senha = model.password;

                    service.Save(user);

                }

                TempData["Sucesso"] = true;
                TempData["Mensagem"] = "Usuário Cadastrado com Sucesso: " + model.email + " !";

                var view = RedirectToAction("Index");
                view.ExecuteResult(ControllerContext);

            }
            catch (Exception exception)
            {
                TempData["Erro"] = true;
                TempData["Mensagem"] = exception.Message;

                var view = RedirectToAction("Cadastro");
                view.ExecuteResult(ControllerContext);
            }

            return null;
        }

        [HttpPost]
        public ActionResult Senha(string email)
        {
            try
            {

                if (email == null)
                    throw new Exception("Preencha o campo email");

                if (email == "")
                    throw new Exception("Preencha o campo email");

                using (var service = new Service<Usuario>())
                {
                    var y = service.GetRepository().Where(x => x.Email == email).ToList();
                    if (y.Count > 0)
                    {
                        MailMessage mail = new MailMessage("kkaio.rocha64@gmail.com", email);
                        SmtpClient client = new SmtpClient();
                        client.Port = 587;
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        client.UseDefaultCredentials = false;
                        client.Host = "smtp.gmail.com";
                        mail.Subject = "EcoCharge - Sua Senha";
                        mail.Body = "Senha para LOGIN - " + y.First().Senha;
                        client.Send(mail);

                    }
                }

                TempData["Sucesso"] = true;
                TempData["Mensagem"] = "Senha enviada com sucesso para o email: " + email + " !";

            }
            catch (Exception exception)
            {
                TempData["Erro"] = true;
                TempData["Mensagem"] = exception.Message;
            }

            var view = RedirectToAction("Senha");
            view.ExecuteResult(ControllerContext);

            return null;
        }
    }

}