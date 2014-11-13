using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Postal;
using Easy.Models;

namespace Easy.Controllers
{
    public class EmailController : Controller
    {
        //
        // GET: /Email/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EnviarEmailCompromisso(Compromissos comp, string participantes)
        {
            var email = new EmailCompromisso
            {
                Para = participantes,
                Assunto = "Compromisso -"+ comp.Titulo,
                Titulo = comp.Titulo,
                Descricao = comp.Descricao,
                Quando = "Início em: " + comp.DataInicio,
                Mensagem = "Este evento é patrocinado por Easy Peoples",
                UrlAceita = "http://localhost:0000/Compromissos/UrlConfirma?codigo=",
                UrlRejeita = "http://google.com"
            };
            try
            {

                email.Send();
                return RedirectToAction("Index", "Home");

            }
            catch
            {

                return RedirectToAction("Index", "Calendario");

            }
        }

        [HttpPost]
        public ActionResult EnviarEmailSistema()
        {
            var email = new EmailSistema
            {
                Para = "vitor_hs@live.com",
                Assunto = "Bem Vindo",
                Titulo = "Bem Vindo ao Easy Peoples",
                Descricao = "Você foi convidado, a participar de nosso Sistema. Abaixo seus dados de Acesso Inicial:",
                Login = "vitor_hs@live.com",
                Senha = "google123",
                Mensagem = "Você pode alterar sua senha a qualquer momento, basta acessar seu perfil"

            };

            try
            {

                email.Send();
                return RedirectToAction("Index", "Home");

            }
            catch
            {

                return RedirectToAction("Index", "Calendario");

            }
        }

        [HttpPost]
        public ActionResult EnviarEmailRecuperar(Usuario user)
        {

            var email = new EmailRecuperar
            {
                Para = user.Email,
                Assunto = "Recuperar - Nova Senha",
                Titulo = "Bem Vindo ao Easy Peoples",
                Descricao = "Recentemente foi solicitado um pedido de alteração de senha. Abaixo seus novos dados de Acesso:",
                Login = user.Email,
                Senha = user.Senha,
                Mensagem = "Você pode alterar sua senha a qualquer momento, basta acessar seu perfil"

            };

            try
            {

                email.Send();
                return RedirectToAction("Index", "Login");

            }
            catch
            {

                return RedirectToAction("Recuperar", "Login");

            }
        }





























        //public ActionResult EmailSistemaPreview()
        //{
        //    var email = new EmailSistema
        //    {
        //        Para = "vitor_hs@live.com",
        //        Assunto = "Bem Vindo",
        //        Titulo = "Bem Vindo ao Easy Peoples",
        //        Descricao = "Você foi convidado, a participar de nosso Sistema. Abaixo seus dados de Acesso Inicial:",
        //        Login = "vitor_hs@live.com",
        //        Senha = "google123",
        //        Mensagem = "Você pode alterar sua senha a qualquer momento, basta acessar seu perfil"
        //    };

        //    return new EmailViewResult(email);
        //}
    }
    }

