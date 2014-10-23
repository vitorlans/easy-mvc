using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Easy.Models;

namespace Easy.Controllers
{
    public class ContatosController : Controller
    {
        //
        // GET: /Contatos/

        public ActionResult Index(Usuario user)
        {
            DAOUsuario DUser = new DAOUsuario();

            var lista = DUser.ListaUsuarios();
            return View(lista);
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

            DUser.CriarUsuario(user, "1");

            return View();

        }
        public ActionResult Detalhes(string id)
        {
            DAOUsuario DUser = new DAOUsuario();

            var user = DUser.RecuperaUsuario(id);
            

            return View(user);

        }
    }
}
