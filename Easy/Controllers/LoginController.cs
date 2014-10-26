using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Easy.Models;

namespace Easy.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/

        [HttpGet]
        public JsonResult AutenticacaoDeUsuario(string email, string senha, string Url)
        {

            if (Usuario.AutenticarUsuario(email, senha))
            {
                if (Url != null)
                {
                    DAOUsuario DUser = new DAOUsuario();
                    Session["Usuario"] = DUser.RecuperaUsuarioEmail(email);
                    Session.Timeout = 1440;

                    return Json(new
                    {
                        OK = true,
                        Mensagem = "Usuário Autenticado. Redirecionando...",
                        Local = Url
                    },
                    JsonRequestBehavior.AllowGet);
                }
                else {

                    DAOUsuario DUser = new DAOUsuario();
                    Session["Usuario"] = DUser.RecuperaUsuarioEmail(email);
                    Session.Timeout = 1440;

                    return Json(new
                    {
                        OK = true,
                        Mensagem = "Usuário Autenticado. Redirecionando...",
                        Local = "/Home/Index"
                    },
                    JsonRequestBehavior.AllowGet);
                    
                }
            }
            else
            {
                return Json(new
                {
                    OK = false,
                    Mensagem = "Usuário não encontrado. Tente Novamente."
                },
                JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Deslogar()
        {
            Session.Clear();
            Session["Usuario"] = null;
            Autenticacao.RemoverCookieAutenticacao();

            return RedirectToAction("Index");
        }

        public ActionResult Index()
        {
            return View();
        }

    }
}
