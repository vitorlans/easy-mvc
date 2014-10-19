using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Easy.Models
{
    public class Tarefas
    {
        [Display(Name = "Descrição")]
        public string   Descricao   { get; set; }
        [Display(Name = "Data de Início")]
        public string   DtInicio    { get; set; }
        [Display(Name = "Data de Término")]
        public string   DtFim       { get; set; }
        public string   Prioridade  { get; set; }
        public string   Status      { get; set; }
        public Usuario  Criador     { get; set; }
        public Usuario  Relacionado { get; set; }
    }
}