﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        //Status será inserido como (T)erminado, (C)ancelado ou em (A)ndamento
        public string Status {get;set;}
        public Usuario Usuario {get;set;}
        public Empresas Empresa { get; set; }     
    }
}