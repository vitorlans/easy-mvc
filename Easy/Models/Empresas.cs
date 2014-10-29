using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Easy.Models
{
    public class Empresas
    {
        public int IdEmpresa { get; set; }
        public string CnpjEmpresa { get; set; }
        public string NomeEmpresa { get; set; }
        public string StatusEmpresa { get; set; }


        public static Empresas RecuperaEmpresaCookie()
        {
            DAOEmpresas DEmp = new DAOEmpresas();
            var Empresa = HttpContext.
            Current.
            Request.
            Cookies["UserCookieEmpresa"];
            if (Empresa == null)
            {
                return null;
            }
            else
            {
                string IDEmpresa = Criptografia.Descriptografar(Empresa.Values["IDEMPRESA"]);
                var UsuarioRetornado = DEmp.RecuperarEmpresaId(IDEmpresa);
                return UsuarioRetornado;
            }
        }
    }

    public class VinculoEmpresa
    {

        public int IdEmp { get; set; }
        public int IdUser { get; set; }


    }
}