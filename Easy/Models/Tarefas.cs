using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Easy.Models
{
    public class Tarefas
    {
        public string   Descricao   { get; set; }
        public string   DtInicio    { get; set; }
        public string   DtFim       { get; set; }
        public string   Prioridade  { get; set; }
        public string   Status      { get; set; }
        public Usuario  Criador     { get; set; }
        public Usuario  Relacionado { get; set; }
    }
}