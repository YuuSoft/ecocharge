using BLL.Service;
using Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcoCharge.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: Usuario
        public ActionResult Index()
        {
            try
            {
                using (var service = new Service<Usuario>())
                {
                    var id_usuario = Convert.ToInt32(Session["UserId"]);

                    var usuario = service.FindById(new Usuario() { Id = id_usuario });

                    ViewBag.nome = usuario.Nome;
                    ViewBag.sobrenome = usuario.Sobrenome;
                    ViewBag.email = usuario.Email;
                    ViewBag.senha = usuario.Senha;
                    ViewBag.telefone = usuario.Telefone;
                    ViewBag.data_de_nascimento = usuario.DataNascimento.ToString("yyyy-MM-dd");
                    ViewBag.genero = usuario.Genero;
                    ViewBag.foto = usuario.Foto;
                    Session["UserFoto"] = usuario.Foto;
                    ViewBag.qtde_pessoas_na_casa = usuario.QtdePessoasCasa;
                    ViewBag.qual_comodo_mais_gasta = usuario.Qualcomodomaisgasta;
                    ViewBag.valor_conta_luz = usuario.ValorContaLuz;
                }
            }
            catch (Exception exception)
            {
                TempData["Erro"] = true;
                TempData["Mensagem"] = "Mantenha seu perfil preenchidos para valores mais exatos";
            }
           
            return View();
        }

        [HttpPost]
        public ActionResult Cadastrar(Usuario model)
        {

            try
            {
                using (var service = new Service<Usuario>())
                {
                    var id_usuario = Convert.ToInt32(Session["UserId"]);

                    var usuario = service.FindById(new Usuario() { Id = id_usuario });

                    var image = model.Telefone;
                    usuario.Nome = model.Nome;
                    usuario.Sobrenome = model.Sobrenome;
                    usuario.Email = model.Email;
                    usuario.Senha = model.Senha;
                    usuario.Telefone = model.Telefone;
                    usuario.DataNascimento = model.DataNascimento;
                    usuario.Genero = model.Genero;
                    usuario.Foto =  model.Foto;
                    usuario.QtdePessoasCasa = model.QtdePessoasCasa;
                    usuario.Qualcomodomaisgasta = model.Qualcomodomaisgasta;
                    usuario.ValorContaLuz = model.ValorContaLuz;

                    service.Save(usuario);

                    ViewBag.nome = usuario.Nome;
                    ViewBag.sobrenome = usuario.Sobrenome;
                    ViewBag.email = usuario.Email;
                    ViewBag.senha = usuario.Senha;
                    ViewBag.telefone = usuario.Telefone;
                    ViewBag.data_de_nascimento = usuario.DataNascimento.ToString("yyyy-MM-dd");
                    ViewBag.genero = usuario.Genero;
                    ViewBag.foto = usuario.Foto;
                    ViewBag.qtde_pessoas_na_casa = usuario.QtdePessoasCasa;
                    ViewBag.qual_comodo_mais_gasta = usuario.Qualcomodomaisgasta;
                    ViewBag.valorContaLuz = usuario.ValorContaLuz;
                }

                TempData["Sucesso"] = true;
                TempData["Mensagem"] = "Dados cadastrado com sucesso";

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