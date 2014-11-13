using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Easy.Models;

namespace Easy.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            Session["Usuario"] = (object)Usuario.VerificaSeOUsuarioEstaLogado() ?? "Ninguem";

            return View();
        }

        public ActionResult Empresa() {



            return View();
        
        }

        [HttpPost]
        public ActionResult Empresa(Empresas emp)
        {
            if (emp.NomeEmpresa != null)
            {
                Autenticacao.RegistrarEmpresaCookie(emp);
                Session["Empresa"] = emp;
            }
            return RedirectToAction("Index","Home");

        }

        public ActionResult Perfil()
        {
            Usuario user = Usuario.VerificaSeOUsuarioEstaLogado();

            return View(user);
        }

        [HttpPost]
        public ActionResult Perfil(Usuario user)
        {

            DAOUsuario duser = new DAOUsuario();
            duser.AtualizarUsuario(user);
            return View(user);
        }

        [HttpPost]
        public ActionResult AlterarSenha(Usuario user)
        {

            DAOUsuario duser = new DAOUsuario();
            duser.AlterarSenha(user);
            return RedirectToAction("Index", "Home");
        }
    }
}
