using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Easy.Models
{
    public class Compromissos
    {
        public int IdComp {get;set;}
        public string Titulo {get;set;}
        public string Descricao {get;set;}
        public DateTime DataInicio {get;set;}
        public DateTime DataTermino {get;set;}
        public Boolean Status {get;set;}
        public Usuario Usuario{get;set;}
    }
}