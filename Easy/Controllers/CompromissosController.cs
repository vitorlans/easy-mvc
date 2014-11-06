﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Easy.Models;

namespace Easy.Controllers
{
    public class CompromissosController : BaseController
    {
        //
        // GET: /Eventos/

        public ActionResult Index()
        {
            DAOCompromissos daoCompromisso = new DAOCompromissos();
            var listaComp = daoCompromisso.ListarCompromissosData();
            return View(listaComp);
        }
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(Compromissos Compromisso, string participantes)
        {
            if (Compromisso.Titulo != null && Compromisso.Descricao != null && Compromisso.DataInicio != null && Compromisso.DataTermino != null)
            {
                //Compromisso.Usuario = (Usuario)Session["Usuario"];
                Usuario User = Usuario.VerificaSeOUsuarioEstaLogado();
                Compromisso.Usuario = User;

                Empresas Emp = Empresas.RecuperaEmpresaCookie();
                Compromisso.Empresa = Emp;

                //Quebrar email por ponto e virgula e  add ao compromisso
                Compromisso = Compromissos.SplitParticipantes(Compromisso,participantes);                
                
                DAOCompromissos daoCompromisso = new DAOCompromissos();
                daoCompromisso.AddCompromisso(Compromisso);
                Compromisso.IdComp = daoCompromisso.RecuperaIdUltimoCompromisso(Compromisso.Usuario.IdUser);
                if (Compromisso.Participantes != null)
                    for(int x = 0; x <= Compromisso.Participantes.Count;x++)
                        daoCompromisso.AddParticipantesCompromisso(Compromisso, Compromisso.Participantes[x]);

                Session["AddCompromisso"] = 1;
                Session.Timeout = 1;
            }
            else
            {
                Session["AddCompromisso"] = 0;
                Session.Timeout = 1;
            }
            return RedirectToAction("Index", "Compromissos");
        }
        [HttpGet]
        public ActionResult EditarCompromisso(int id)
        {
            DAOCompromissos daoCompromisso = new DAOCompromissos();
            Compromissos Compromisso = daoCompromisso.SelecionarCompromissoId(id);
            Usuario User = Usuario.VerificaSeOUsuarioEstaLogado();
            if (Compromisso.Usuario.IdUser == User.IdUser)
                return View(Compromisso);
            else
                return RedirectToAction("Index", "Compromissos");
        }
        [HttpPost]
        public ActionResult AtualizarCompromisso(Compromissos CompromissoNovo)
        {
            if (ModelState.IsValid)
            {
                DAOCompromissos dCompromisso = new DAOCompromissos();
                Usuario User = Usuario.VerificaSeOUsuarioEstaLogado();
                dCompromisso.EditarCompromisso(CompromissoNovo, User);
                Session["AddCompromisso"] = 2;
                Session.Timeout = 1;
            }
            else
            {
                Session["AddCompromisso"] = 0;
                Session.Timeout = 1;
            }
            return RedirectToAction("Index", "Compromissos");

        }
        [HttpPost]
        public ActionResult AlterarStatusComp(int id)
        {
            DAOCompromissos dCompromisso = new DAOCompromissos();
            Compromissos Compromisso = new Compromissos();
            Compromisso = dCompromisso.SelecionarCompromissoId(id);

            //Usuario User = (Usuario)Session["Usuario"];
            Usuario User = Usuario.VerificaSeOUsuarioEstaLogado();

            dCompromisso.AlterarStatusComp(Compromisso, User);
            return RedirectToAction("Index", "Compromissos");
        }

        [HttpGet]
        public ActionResult UrlConfirma(string codigo, int id) {


            return View();
        
        
        }
    }
}
