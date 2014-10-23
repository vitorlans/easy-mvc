using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Easy.Models
{
    public class Notas
    {
        public int IdNota { get; set; }
        public string DescricaoNota { get; set; }
        public Usuario Usuario { get; set; }
        public Empresas Empresa { get; set; }
    }
}