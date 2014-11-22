using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Easy.Models;

namespace Easy.Controllers
{
    public class EmpresaController : Controller
    {
        //
        // GET: /Empresa/

        public ActionResult Index()
        {
            if (Usuario.VerificaSeOUsuarioEstaLogado() == null)
            {
                return Redirect("/Login");
            }

            return View();

        }

        [HttpPost]
        public ActionResult Index(Empresas emp, string Url)
        {
            if (emp.NomeEmpresa != null)
            {
                Autenticacao.RegistrarEmpresaCookie(emp);
                Session["Empresa"] = emp;
            }
            if (Url != null)
            {
                ParametrosSistema p = new ParametrosSistema();
                if (p.ListaParametros()[0].ValorParam == true && ( Url == "/" || Url == "/Home/Index"))
                {
                    Url = "/Calendario";
                }

                return Redirect(Url);
            }
            else
            {
                ParametrosSistema p = new ParametrosSistema();
                if (p.ListaParametros()[0].ValorParam == true) {
                    return RedirectToAction("Index", "Calendario");
                
                }

                return RedirectToAction("Index", "Home");
            }
        }

    }
}
