using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Easy.Models;
using PagedList;

namespace Easy.Controllers
{
    public class CompromissosController : BaseController
    {
        //
        // GET: /Compromissos/

        public ActionResult Index(string sortOrder, string search, int? page, string currentFilter)
        {
            if (search != null)
                page = 1;

            else
                search = currentFilter;

            ViewBag.Filtro = search;

            DAOCompromissos daoCompromisso = new DAOCompromissos();
            var listaComp = daoCompromisso.ListarCompromissosData();
            var x = from lista in listaComp select lista;
            
            if (!String.IsNullOrEmpty(search))
            {
                x = x.Where(lista => lista.Titulo.ToUpper().Contains(search.ToUpper())); //PODE-SE ACRESCENTAR MAIS CLAUSULAS
            }

            switch (sortOrder)
            {
                case "icresc":
                    x = x.OrderBy(lista => lista.DataInicio);
                    break;
                case "idesc":
                    x = x.OrderByDescending(lista => lista.DataInicio);
                    break;
                case "tcresc":
                    x = x.OrderBy(lista => lista.DataTermino);
                    break;
                case "tdesc":
                    x = x.OrderByDescending(lista => lista.DataTermino);
                    break;
                case "cancelado":
                    x = x.Where(lista => lista.Status.ToUpper().Equals("C"));
                    break;
                case "concluido":
                    x = x.Where(lista => lista.Status.ToUpper().Equals("T"));
                    break;
                case "proximo":
                    x = x.Where(lista => lista.Status.ToUpper().Equals("P"));
                    break;
                case "andamento":
                    x = x.Where(lista => lista.Status.ToUpper().Equals("A"));
                    break;
                default:
                    x = x.OrderBy(lista => lista.DataInicio);
                    break;
            }

            listaComp = x.ToList();

            for (int y = 0; y < listaComp.Count; y++)
            {

                string dtIni = Convert.ToDateTime(listaComp[y].DataInicio).ToShortDateString();
                string dtFim = Convert.ToDateTime(listaComp[y].DataTermino).ToShortDateString();
                string hrIni = Convert.ToDateTime(listaComp[y].DataInicio).ToShortTimeString();
                string hrFim = Convert.ToDateTime(listaComp[y].DataTermino).ToShortTimeString();



                if (dtIni == dtFim)
                {
                    listaComp[y].DataInicio = Convert.ToDateTime(listaComp[y].DataInicio.ToString()).ToLongDateString() + " às " + hrIni.ToString() + " até ";
                    listaComp[y].DataInicio = Compromissos.FormataTexto(listaComp[y].DataInicio);
                    listaComp[y].DataTermino = hrFim;
                }
                else
                {
                    listaComp[y].DataInicio = Convert.ToDateTime(listaComp[y].DataInicio.ToString()).ToLongDateString() + " às " + hrIni.ToString() + " até ";
                    listaComp[y].DataInicio = Compromissos.FormataTexto(listaComp[y].DataInicio);
                    listaComp[y].DataTermino = Convert.ToDateTime(listaComp[y].DataTermino.ToString()).ToLongDateString() + " às " + hrIni.ToString();
                    listaComp[y].DataTermino = Compromissos.FormataTexto(listaComp[y].DataTermino);
                }
            }
           
            x = from lista in listaComp select lista;
            int pageSize = 3;
            int pageNumber = (page ?? 1);

            return View(x.ToPagedList(pageNumber, pageSize));
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
                try
                {
                    //Compromisso.Usuario = (Usuario)Session["Usuario"];
                    Usuario User = Usuario.VerificaSeOUsuarioEstaLogado();
                    Compromisso.Usuario = User;

                    Empresas Emp = Empresas.RecuperaEmpresaCookie();
                    Compromisso.Empresa = Emp;

                    //Quebrar email por ponto e virgula e  add ao compromisso
                    if (participantes != "")
                    {
                        Compromisso = Compromissos.SplitParticipantes(Compromisso, participantes);
                        //Compromisso.Participantes.Count(
                    }

                    DAOCompromissos daoCompromisso = new DAOCompromissos();
                    daoCompromisso.AddCompromisso(Compromisso);
                    Compromisso.IdComp = daoCompromisso.RecuperaIdUltimoCompromisso(Compromisso.Usuario.IdUser);
                    if (Compromisso.Participantes[0] != null)
                        for (int x = 0; x <= Compromisso.Participantes.Count; x++)
                            daoCompromisso.AddParticipantesCompromisso(Compromisso, Compromisso.Participantes[x]);

                    EmailController EmailC = new EmailController();
                    EmailC.EnviarEmailCompromisso(Compromisso, participantes);
                    Session["AddCompromisso"] = 1;
                    Session.Timeout = 1;
                }
                catch
                {
                    Session["AddCompromisso"] = 0;
                    Session.Timeout = 1;
                }
            }
            else
            {
                Session["AddCompromisso"] = 0;
                Session.Timeout = 1;
            }
            return RedirectToAction("Index", "Compromissos");

        }
        [HttpGet]
        public ActionResult EditarCompromisso(string id)
        {
            int id1;
            if (int.TryParse(id, out id1))
            {
                DAOCompromissos daoCompromisso = new DAOCompromissos();
                Compromissos Compromisso = daoCompromisso.SelecionarCompromissoId(id1);
                Usuario User = Usuario.VerificaSeOUsuarioEstaLogado();
                Empresas Emp = Empresas.RecuperaEmpresaCookie();
                Compromisso.Empresa = Emp;
                string participantes = "";
                int x = 0;
                while (x < Compromisso.Participantes.Count)
                {
                    participantes += Compromisso.Participantes[x].Email.ToString() + ";";
                    x++;
                }
                //if (Compromisso.IdComp != 0 && Compromisso.Usuario.IdUser == User.IdUser)
                ViewBag.participantes = participantes;
                    return View(Compromisso);
                /*else
                    return RedirectToAction("Index", "Compromissos");*/
            }
            else
                return RedirectToAction("Index", "Compromissos");
        }
        [HttpPost]
        public ActionResult AtualizarCompromisso(Compromissos CompromissoNovo, string participantes, string id)
        {
            if (CompromissoNovo.Titulo != null && CompromissoNovo.Descricao != null && CompromissoNovo.DataInicio != null && CompromissoNovo.DataTermino != null)
            {
                try
                {
                    //Compromisso.Usuario = (Usuario)Session["Usuario"];
                    Usuario User = Usuario.VerificaSeOUsuarioEstaLogado();
                    CompromissoNovo.Usuario = User;

                    Empresas Emp = Empresas.RecuperaEmpresaCookie();
                    CompromissoNovo.Empresa = Emp;

                    //Quebrar email por ponto e virgula e  add ao compromisso
                    if (participantes != "")
                    {
                        CompromissoNovo = Compromissos.SplitParticipantes(CompromissoNovo, participantes);
                        //Compromisso.Participantes.Count(
                    }

                    DAOCompromissos daoCompromisso = new DAOCompromissos();
                    daoCompromisso.EditarCompromisso(CompromissoNovo, User);
                    if (CompromissoNovo.Participantes[0] != null)
                        for (int x = 0; x <= CompromissoNovo.Participantes.Count; x++)
                        {
                            if(CompromissoNovo.Participantes[x] != null)
                                daoCompromisso.AddParticipantesCompromisso(CompromissoNovo, CompromissoNovo.Participantes[x]);
                        }
                    EmailController EmailC = new EmailController();
                    EmailC.EnviarEmailCompromisso(CompromissoNovo, participantes);
                    Session["AddCompromisso"] = 2;
                    Session.Timeout = 1;
                }
                catch
                {
                    Session["AddCompromisso"] = 0;
                    Session.Timeout = 1;
                }
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
            return RedirectToAction("EditarCompromisso", "Compromissos");
        }

        [HttpGet]
        public ActionResult UrlConfirma(string codigo, int id) {


            return View();
        
        
        }

        [HttpPost]
        public ActionResult AddCal(string data1)
        {

            Compromissos comp = new Compromissos();
            var dt = Convert.ToDateTime(data1);
            var dt1 = dt.AddHours(1);

            comp.DataInicio = dt.ToString();
            comp.DataTermino = dt1.ToString();

            return View("Add", comp);
        }
    }
}
