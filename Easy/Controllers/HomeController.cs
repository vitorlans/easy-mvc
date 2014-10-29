using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Easy.Models;

namespace Easy.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Session["Usuario"] = Usuario.VerificaSeOUsuarioEstaLogado() ;

            return View();
        }

        public ActionResult Empresa() {



            return View();
        
        }

        [HttpPost]
        public ActionResult Empresa(Empresas emp)
        {
            Autenticacao.RegistrarEmpresaCookie(emp);
            Session["Empresa"] = emp;

            return RedirectToAction("Index","Home");

        }
    }
}
