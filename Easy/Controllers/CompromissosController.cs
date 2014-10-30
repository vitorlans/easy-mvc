using System;
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
        public ActionResult Add(Compromissos Compromisso)
        {
            if (ModelState.IsValid)
            {
                string login = Compromisso.Usuario.Email;
                DAOUsuario daoUsuario = new DAOUsuario();
                Compromisso.Usuario = daoUsuario.RecuperaUsuarioEmail(login);
                Compromisso.Status = "A";
                //retirar qdo obter o id da empresa
                DAOEmpresas daoEmpresa = new DAOEmpresas();
                Compromisso.Empresa = daoEmpresa.RecuperarEmpresaId("1");
                DAOCompromissos daoCompromisso = new DAOCompromissos();
                daoCompromisso.AddCompromisso(Compromisso);
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
        [HttpPost]
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
            DAOCompromissos dCompromisso = new DAOCompromissos();
            Usuario User = Usuario.VerificaSeOUsuarioEstaLogado();
            dCompromisso.EditarCompromisso(CompromissoNovo, User);
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
    }
}
