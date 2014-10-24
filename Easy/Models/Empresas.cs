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
    }
}