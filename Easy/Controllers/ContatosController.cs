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
            var lista = user.ContatosLTotal();
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

                user.UsuarioSistema = false;
            }
            else
            {

                user.UsuarioSistema = true;
            }

            if (CheckEnvConv == null)
            {
                user.LiberaConvite = false;
            }
            else
            {

                user.LiberaConvite = true;
            }

            
            return View();

        }
        public ActionResult Detalhes(string id)
        {

            Usuario user = new Usuario();
            user.UsuarioSistema = true;
            user.Nome = "Vitor Santos";
            user.Sobrenome = "Silva";
            user.Telefone = "19 3582-5664";
            user.Email = "vitor_hs@live.com";
            user.Endereco = "Mauricio Affonso Moreno";
            user.Cidade = "Santa Rita do Passa Quatro";
            user.Bairro="Vila Norte";
            user.Cep="13670000";
            user.LiberaConvite=true;

            return View(user);

        }
    }
}
