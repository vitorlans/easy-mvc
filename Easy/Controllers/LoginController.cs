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
        public JsonResult AutenticacaoDeUsuario(string email, string senha, string Url, string Ip)
        {
            if (Autenticacao.RecuperaIPCookie() != Ip)
            {

                if (Usuario.AutenticarUsuario(email, senha))
                {
                    if (Url != null)
                    {
                        return Json(new
                        {
                            OK = true,
                            Mensagem = "Usuário Autenticado. Redirecionando...",
                            Local = Url
                        },
                        JsonRequestBehavior.AllowGet);
                    }
                    else
                    {

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
                    if (true)
                    {
                        string block = (string)Session["IPBlock"] ?? "0";

                        Int32 blk = Int32.Parse(block);
                        blk++;
                        
                        Session["IPBlock"] = blk.ToString();

                        if (blk > 9)
                        {

                            Autenticacao.BloqueiaAcesso(Ip);
                            Session["IPBlock"] = "0";
                            blk = 0;

                            return Json(new
                            {
                                OK = false,
                                Mensagem = "Seu acesso ao sistema foi Bloqueado. Devido a varias Tentativas. Tente mais tarde ou entre em contato com Adminstração"
                            },
                             JsonRequestBehavior.AllowGet);

                        }

                    };

                    return Json(new
                    {
                        OK = false,
                        Mensagem = "Usuário não encontrado. Tente Novamente."
                    },
                    JsonRequestBehavior.AllowGet);
                }
            }
            else {

                return Json(new
                {
                    OK = false,
                    Mensagem = "Seu acesso ao sistema foi suspenso. Devido a varias tentativas sem sucesso ! Tente acesso mais tarde ou Entre em contato com Adminstração",
                    Cor = true
                },
                             JsonRequestBehavior.AllowGet);

            }
        }

        public ActionResult Deslogar()
        {
            Session.Clear();
            Session["Usuario"] = null;
            Autenticacao.RemoverCookieAutenticacao();

            Session["snackl"] = "2";

            return RedirectToAction("Index");
        }

        public ActionResult Recuperar() {

            return View();
        }
        
        [HttpPost]
        public ActionResult Recuperar(string email)
        {
            DAOUsuario DUser = new DAOUsuario();
            var user = DUser.RecuperaUsuarioEmail(email);
            user.Senha = Criptografia.GerarSenha();
            if (user.Email != null)
            {
                DUser.AlterarSenha(user.IdUser.ToString(), user.Senha);
                   EmailController c = new EmailController();
                    c.EnviarEmailRecuperar(user);
                    Session["snackl"] = "1";
                    return RedirectToAction("Index", "Login");
                }
                else {
                    Session["snackl"] = "3";
                    return RedirectToAction("Index", "Login");
                }
        }

        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public ActionResult UrlConfirma(string codigo)
        {

           
            return Content("Participação Confirmada");
        }

        [HttpGet]
        public ActionResult UrlRejeita(string codigo)
        {

            char[] strSep1 = { '-' }; //Caracter usado para separar o texto
            var strArray = codigo.Split(strSep1);//Onde ficará o resultado da separação

            int IdComp = int.Parse(strArray[0]);
            int IdUser = int.Parse(strArray[1]);
            DAOCompromissos dComp = new DAOCompromissos();
            dComp.RemoverVincPartEmail(IdComp, IdUser);

            return Content("Participação Retirada");
        }

    }
}
