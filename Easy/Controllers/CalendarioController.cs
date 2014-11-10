using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Easy.Models;

namespace Easy.Controllers
{
    public class CalendarioController : BaseController
    {
        //
        // GET: /Calendario/

        public ActionResult Index()
        {
            return View();
        }


        public JsonResult RecuperarEventos(string start, string end)
        {

            DAOCompromissos DAOcomp = new DAOCompromissos();
            var LstComp = DAOcomp.ListarCompromissosTodos();
                         List<Calendario> Lc = new List<Calendario>();

                         int x = 0;

            foreach(var i in LstComp) {
                var dt = string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(LstComp[x].DataInicio));
                var dt2 = string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(LstComp[x].DataTermino));

                var hr = Convert.ToDateTime(LstComp[x].DataInicio).ToShortTimeString();
                var hr2 = Convert.ToDateTime(LstComp[x].DataTermino).ToShortTimeString();

                 var cd = dt+"T"+hr;
                 var cd2 = dt2 + "T"+hr2;

                Lc.Add(
                    new Calendario
                    {
                        id= LstComp[x].IdComp.ToString(), 
                        allday = false,
                        title=  "Comp: "+LstComp[x].Titulo,
                        description = LstComp[x].Descricao,
                        start = cd,
                        end = cd2,
                        url = "http://localhost:58623/Compromissos/EditarCompromisso?id="+LstComp[x].IdComp.ToString(),
                        color = "pink"

                    }
                );
                x++;
                 
            }


            DAOTarefas DAOTaref = new DAOTarefas();
            var LstTaref = DAOTaref.ListaTarefas();

            int y = 0;

            foreach (var i in LstTaref)
            {
                var dt = string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(LstTaref[y].DtInicio));
                var dt2 = string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(LstTaref[y].DtFim));

                //var hr = Convert.ToDateTime(LstTaref[y].DtInicio).ToShortTimeString();
                //var hr2 = Convert.ToDateTime(LstTaref[y].DtFim).ToShortTimeString();
                //if (hr == "00:00" && hr2 == "00:00")
                //{
                //    hr = "12:30";
                //    hr2 = "13:30";
                //}

                //var cd = dt + "T" + hr;
                //var cd2 = dt2 + "T" + hr2;

                Lc.Add(
                    new Calendario
                    {
                        id = LstTaref[y].IdTarefa.ToString(),
                        title = "Tarefa :"+LstTaref[y].Descricao,
                        start = dt,
                        end = dt2,
                        url = "http://localhost:58623/Tarefas/EditTarefa?id=" + LstTaref[y].IdTarefa.ToString(),
                        allday = true,
                        borderColor ="#5173DA",
                        color = "#99ABEA"
                        
                    }
                );
                y++;

            }

            var row = Lc.ToArray();
            return Json(row, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult NovoCompromisso(string dia)
        {
            return Json(new
            {
                OK = true,
                Mensagem = "Usuário Autenticado. Redirecionando...",
                Local = "112"
            },JsonRequestBehavior.AllowGet);
            
        }

    }
}
