using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Easy.Models;

namespace Easy.Controllers
{
    public class ContatosController : BaseController
    {
        //
        // GET: /Contatos/

        public ActionResult Index(Usuario user)
        {
           
            Usuario Logado = Usuario.VerificaSeOUsuarioEstaLogado();
            DAOUsuario DUser = new DAOUsuario();
            List<Usuario> lista = new List<Usuario>();
            if (Logado != null)
            {

                lista = DUser.ListaUsuarios(Logado.IdUser.ToString());
            }
            var x = lista.OrderBy(m => m.Nome);
            return View(x);
        }

        [HttpPost]
        public ActionResult Index(string pesquisa)
        {

            if (pesquisa == "")
            {
                Usuario Logado = Usuario.VerificaSeOUsuarioEstaLogado();
                DAOUsuario DUser = new DAOUsuario();

                var lista = DUser.ListaUsuarios(Logado.IdUser.ToString());
                return View(lista);
            }
            else {

                Usuario Logado = Usuario.VerificaSeOUsuarioEstaLogado();
                DAOUsuario DUser = new DAOUsuario();

                var lista = DUser.ListaUsuariosPesquisa(Logado.IdUser.ToString(), pesquisa);
                return View(lista);
                
            
            }

        }
    
        [HttpGet]
        public ActionResult Alfa(string letra)
        {
            if (letra != null)
            {
                letra = letra.ToUpper();

                if (letra == "A" || letra == "B" || letra == "C" || letra == "D" || letra == "E" || letra == "F" || letra == "G" || letra == "H" || letra == "I" || letra == "J" || letra == "K" || letra == "L" || letra == "M" || letra == "N" || letra == "O" || letra == "P" || letra == "Q" || letra == "R" || letra == "S" || letra == "T" || letra == "U" || letra == "V" || letra == "W" || letra == "X" || letra == "Y" || letra == "Z")
                {

                    Usuario Logado = Usuario.VerificaSeOUsuarioEstaLogado();
                    DAOUsuario DUser = new DAOUsuario();

                    var lista = DUser.ListaUsuarios(Logado.IdUser.ToString(), letra);
                    return View("Index", lista);

                }
                else
                {

                    Usuario Logado = Usuario.VerificaSeOUsuarioEstaLogado();
                    DAOUsuario DUser = new DAOUsuario();

                    var lista = DUser.ListaUsuarios(Logado.IdUser.ToString());
                    return View("Index", lista);

                }

            }
            else {

                Usuario Logado = Usuario.VerificaSeOUsuarioEstaLogado();
                DAOUsuario DUser = new DAOUsuario();

                var lista = DUser.ListaUsuarios(Logado.IdUser.ToString());
                return View("Index", lista);
            
            }
        }

        public ActionResult Add() {

            return View();
        
        }

        [HttpPost]
        public ActionResult Add(Usuario user, string CheckUserSist, string CheckEnvConv)
        {

            if (CheckUserSist == null)
            {

                user.UsuarioSistema = "N";
            }
            else
            {

                user.UsuarioSistema = "S";
            }

            if (CheckEnvConv == null)
            {
                user.LiberaConvite = "N";
            }
            else
            {

                user.LiberaConvite = "S";
            }

            

            DAOUsuario DUser = new DAOUsuario();
            Usuario Logado = Usuario.VerificaSeOUsuarioEstaLogado();
            var est = DUser.CriarUsuario(user, Logado.IdUser.ToString());

            if (est == true)
            {
                Session["snackc"] = "1";
            }
            else
            {

                Session["snackc"] = "3";

            }
            return RedirectToAction("Index");

        }

        [HttpGet]
        public ActionResult Detalhes() {


            return RedirectToAction("Index", "Contatos");
        }
        [HttpPost]
        public ActionResult Detalhes(string id)
        {
            DAOUsuario DUser = new DAOUsuario();

            var user = DUser.RecuperaContato(id);
            

            return View(user);

        }

        [HttpPost]
        public ActionResult AtualizarDetalhes(Usuario user) {

            DAOUsuario DUser = new DAOUsuario();

            DUser.AtualizarContato(user);
            Session["snack"] = "2";

            return View("Detalhes",user);
        
        }

        [HttpPost]
        public ActionResult Apagar(Usuario user)
        {

            DAOUsuario DUser = new DAOUsuario();

            DUser.ApagarVinculo(user, Usuario.VerificaSeOUsuarioEstaLogado().IdUser.ToString());
            Session["snackc"] = "2";

            return RedirectToAction("Index", "Contatos");

        }

        [HttpPost]
        public ActionResult Permissao(Usuario user, string CheckUserSist, string CheckEnvConv)
        {
            if (CheckUserSist == null)
            {

                user.UsuarioSistema = "N";
            }
            else
            {

                user.UsuarioSistema = "S";
            }

            if (CheckEnvConv == null)
            {
                user.LiberaConvite = "N";
            }
            else
            {

                user.LiberaConvite = "S";
            }

             
            DAOUsuario DUser = new DAOUsuario();
            var atUser = DUser.RecuperaUsuario(user.IdUser.ToString());
            if (atUser.UsuarioSistema != user.UsuarioSistema)
            {
                DUser.AtualizaPermissaoSistema(user);
            }
            if (atUser.LiberaConvite != user.LiberaConvite) {

                DUser.AtualizaPermissaoConvite(user);
            }

            atUser.UsuarioSistema = user.UsuarioSistema;
            atUser.LiberaConvite = user.LiberaConvite;

            Session["snack"] = "1";
            return View("Detalhes", atUser);

        }

        [HttpGet]
        public JsonResult AutenticaEmail(string email)
        {

             DAOUsuario duser = new DAOUsuario();

            if (duser.AutenticaEmail(email))
            {

                return Json(new
                {
                    OK = false
                },
            JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(new
                {
                    OK = true
                },
                JsonRequestBehavior.AllowGet);
            
            }
        }
        
    }
}
