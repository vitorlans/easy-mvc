using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Easy.Models;

namespace Easy.Filters
{
    [HandleError]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class AutorizacaoDeAcesso : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext FiltroDeContexto)
        {
            var Controller = FiltroDeContexto.ActionDescriptor.ControllerDescriptor.ControllerName;
            var Action = FiltroDeContexto.ActionDescriptor.ActionName;

            if (Controller != "Home" || Action != "Login")
            {
                if (Usuario.VerificaSeOUsuarioEstaLogado() == null)
                {
                    FiltroDeContexto.RequestContext.HttpContext.Response.Redirect("/Login?Url=" + FiltroDeContexto.HttpContext.Request.Url.LocalPath);
                }
                else
                {

                    if (Empresas.RecuperaEmpresaCookie() == null)
                    {
                        FiltroDeContexto.RequestContext.HttpContext.Response.Redirect("/Empresa?Url=" + FiltroDeContexto.HttpContext.Request.Url.LocalPath);
                    }

                }
    

            }
        }
    }
}