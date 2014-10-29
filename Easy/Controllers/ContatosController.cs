﻿using System;
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

            var lista = DUser.ListaUsuarios(Logado.IdUser.ToString());
            return View(lista);
        }

        [HttpPost]
        public ActionResult Index(string pesquisa, string tipo)
        {

            if (pesquisa == "")
            {
                return View();
            }
            else
            {

                if (tipo == "0")
                {
                    return View();
                }
                else
                {

                    return View();

                }

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

            DUser.CriarUsuario(user, Logado.IdUser.ToString());

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
