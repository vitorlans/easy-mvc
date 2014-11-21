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
            if (Compromisso.Titulo != null && Compromisso.Descricao != null && Compromisso.DataInicio != null && Compromisso.DataTermino != null && Convert.ToDateTime(Compromisso.DataInicio) < Convert.ToDateTime(Compromisso.DataTermino))
            {
                try
                {
                    Usuario User = Usuario.VerificaSeOUsuarioEstaLogado();
                    Compromisso.Usuario = User;

                    Empresas Emp = Empresas.RecuperaEmpresaCookie();
                    Compromisso.Empresa = Emp;

                    if (participantes != "")
                    {
                        Compromisso = Compromissos.SplitParticipantes(Compromisso, participantes);

                    }

                    DAOCompromissos daoCompromisso = new DAOCompromissos();
                    daoCompromisso.AddCompromisso(Compromisso);
                    Compromisso.IdComp = daoCompromisso.RecuperaIdUltimoCompromisso(Compromisso.Usuario.IdUser);
                    if (Compromisso.Participantes[0] != null)
                        for (int x = 0; x < Compromisso.Participantes.Count; x++)
                            daoCompromisso.AddParticipantesCompromisso(Compromisso, Compromisso.Participantes[x]);

                    EmailController EmailC = new EmailController();
                    EmailC.EnviarEmailCompromisso(Compromisso);
                    Session["AddCompromisso"] = 1;
                    Session.Timeout = 1;
                }
                catch
                {
                    Session["AddCompromisso"] = 5;
                    Session.Timeout = 1;
                }
            }
            else
            {
                Session["AddCompromisso"] = 5;
                Session.Timeout = 1;
            }
            return RedirectToAction("Index", "Compromissos");

        }
        [HttpPost]
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
                
                ViewBag.participantes = participantes;
                    return View(Compromisso);                    
            }
            else
                return RedirectToAction("Index", "Compromissos");
        }
        [HttpGet]
        public ActionResult EditarCompromisso()
        {
            return RedirectToAction("Index", "Compromissos");
        }
        [HttpPost]
        public ActionResult AtualizarCompromisso(Compromissos CompromissoNovo, string participantes, string id)
        {
            if (CompromissoNovo.Titulo != null && CompromissoNovo.Descricao != null && CompromissoNovo.DataInicio != null && CompromissoNovo.DataTermino != null && Convert.ToDateTime(CompromissoNovo.DataInicio) < Convert.ToDateTime(CompromissoNovo.DataTermino))
            {
                try
                {
                    Usuario User = Usuario.VerificaSeOUsuarioEstaLogado();
                    CompromissoNovo.Usuario = User;

                    Empresas Emp = Empresas.RecuperaEmpresaCookie();
                    CompromissoNovo.Empresa = Emp;

                    if (participantes != "")
                    {
                        CompromissoNovo = Compromissos.SplitParticipantes(CompromissoNovo, participantes);
                    }

                    DAOCompromissos daoCompromisso = new DAOCompromissos();
                    daoCompromisso.RemoverVincPart(CompromissoNovo.IdComp);
                    daoCompromisso.EditarCompromisso(CompromissoNovo, User);
                    if (CompromissoNovo.Participantes[0] != null)
                        for (int x = 0; x < CompromissoNovo.Participantes.Count; x++)
                        {
                            if(CompromissoNovo.Participantes[x] != null)
                                daoCompromisso.AddParticipantesCompromisso(CompromissoNovo, CompromissoNovo.Participantes[x]);
                        }

                    EmailController EmailC = new EmailController();
                    EmailC.EnviarEmailCompromisso(CompromissoNovo);
                    Session["AddCompromisso"] = 2;
                    Session.Timeout = 1;
                }
                catch
                {
                    Session["AddCompromisso"] = 5;
                    Session.Timeout = 1;
                }
            }
            else
            {
                Session["AddCompromisso"] = 5;
                Session.Timeout = 1;
            }
            return RedirectToAction("Index", "Compromissos");

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
        [HttpPost]
        public ActionResult CancelarComp(string id)
        {
            Usuario User = Usuario.VerificaSeOUsuarioEstaLogado();
            DAOCompromissos daoComp = new DAOCompromissos();
            Compromissos Comp = new Compromissos();

            Comp = daoComp.SelecionarCompromissoId(int.Parse(id));
            daoComp.CancelarComp(Comp, User);
            DAOCompromissos.VerificaStatusComp(Comp);
            Session["AddCompromisso"] = 3;
            Session.Timeout = 1;
            return RedirectToAction("Index", "Compromissos");
        }
        [HttpPost]
        public ActionResult AtivarComp(string id)
        {
            Usuario User = Usuario.VerificaSeOUsuarioEstaLogado();
            DAOCompromissos daoComp = new DAOCompromissos();
            Compromissos Comp = new Compromissos();

            Comp = daoComp.SelecionarCompromissoId(int.Parse(id));
            daoComp.AtivarComp(Comp, User);
            DAOCompromissos.VerificaStatusComp(Comp);
            Session["AddCompromisso"] = 4;
            Session.Timeout = 1;
            return RedirectToAction("Index", "Compromissos");
        }
        public ActionResult Notas()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddNota(Notas Nota)
        {
            string idcomp = Session["sltnota"].ToString();
            if (Nota.DescricaoNota != null && idcomp != "")
            {
                try
                {
                    DAONotas dNota = new DAONotas();
                    Compromissos Comp = new Compromissos();
                    Comp.IdComp = int.Parse(idcomp);
                    dNota.AdicionarNota(Nota);
                    Nota.IdNota = dNota.RecuperaIdUltimaNota(Nota.Usuario.IdUser);
                    dNota.VincNotaComp(Nota, Comp);
                }
                catch{ }
            }
            return RedirectToAction("Index", "Compromissos");
        }
        public ActionResult ExcluirNota(string id)
        {
            if (id != "")
            {
                DAONotas dNota = new DAONotas();
                dNota.DeletarNota(id);
            }
            return RedirectToAction("Index", "Compromissos");
        }
        [HttpGet]
        public JsonResult EditarNota(string id)
        {
            id = id.Substring(3);

            Notas Nota = new Notas();

            if (id != "")
            {
                DAONotas dNota = new DAONotas();
                Nota = dNota.RecuperaNotaID(int.Parse(id));
            }
            return Json(Nota, JsonRequestBehavior.AllowGet); 
        }
        [HttpPost]
        public ActionResult AtualizarNota(string ntId, string ntDesc)
        {
            if (ntId!= null)
            {
                DAONotas dNota = new DAONotas();
                dNota.AtualizarNota(ntId, ntDesc);
            }
            return RedirectToAction("Index", "Compromissos");
        }
        public JsonResult CapturaId(string idcomp) {

            idcomp = idcomp.Substring(1);
            Session["sltnota"] = idcomp;

            return Json(idcomp, JsonRequestBehavior.AllowGet);
        }

       

    }
}
