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

            Session["snackp"] = "1";
            return View(user);
        }

        [HttpGet]
        public JsonResult AlterarSenha(string id, string senha, string senha1)
        {

            if (senha == senha1 && senha.Length >= 7)
            {
                
                
                    DAOUsuario duser = new DAOUsuario();
                    duser.AlterarSenha(id, senha);

                    return Json(new
                    {
                        OK = true,
                        Mensagem = "Senha Alterada com Sucesso...",
                    },
                    JsonRequestBehavior.AllowGet);
                
                
            }
            else
            {
                return Json(new
                {
                    OK = false,
                    Mensagem = "Senha não coincidem ou Menor que 7 Caracteres. Tente Novamente."
                },
                JsonRequestBehavior.AllowGet);
            }
        }

        
        public ActionResult Configuracoes()
        {
            ParametrosSistema param = new ParametrosSistema();
            
            return View(param.ListaParametros());
        }

        [HttpPost]
        public JsonResult AtualizarParametro(string idParam, bool valorParam)
        {
            ParametrosSistema param = new ParametrosSistema();

            if (valorParam)
                param.AtualizarParametro(int.Parse(idParam), false);
            else
                param.AtualizarParametro(int.Parse(idParam), true);

            return Json(new { success = true });

        }

    }
}

