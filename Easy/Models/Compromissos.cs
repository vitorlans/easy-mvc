using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Easy.Models
{
    public class Compromissos
    {
        public int IdComp {get;set;}
        [Required(ErrorMessage= "O Título do Compromisso deve ser informado.")]
        public string Titulo {get;set;}
        [Required(ErrorMessage= "A Descrição do Compromisso deve ser informada.")]
        public string Descricao {get;set;}
        [Required(ErrorMessage = "A Data de Início do Compromisso deve ser informada.")]
        public string DataInicio {get;set;}
        [Required(ErrorMessage = "A Data de Termino do Compromisso deve ser informada.")]
        public string DataTermino {get;set;}
        //Status será inserido como (T)erminado, (C)ancelado ou em (A)ndamento
        public string Status {get;set;}
        public Usuario Usuario {get;set;}
        public Empresas Empresa { get; set; }     
    }
}